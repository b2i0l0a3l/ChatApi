using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.DTO;
using ChatApi.Core.Entities;
using ChatApi.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ChatApi.Infrastructure.Hubs
{

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IUniteOfWork _unitOfWork;
        private readonly ILogger<ChatHub> _logger;
        
        private static readonly ConcurrentDictionary<string, OnlineUserInfo> _onlineUsers = new();

        public ChatHub(IUniteOfWork unitOfWork, ILogger<ChatHub> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }



        public override async Task OnConnectedAsync()
        {
            try
            {
                string? userId = Context.UserIdentifier;
                string connectionId = Context.ConnectionId;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("⚠️ Connection attempt without user identifier");
                    await base.OnConnectedAsync();
                    return;
                }

                Guid userGuid = Guid.Parse(userId);

                UserConnection? userConnection = await _unitOfWork.userConnection.FindAsync(x => x.UserId == userGuid);
                
                if (userConnection == null)
                {
                    userConnection = new UserConnection
                    {
                        UserId = userGuid,
                        ConnectionId = connectionId,
                        isConnected = true,
                        ConnectedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.userConnection.AddAsync(userConnection);
                }
                else
                {
                    userConnection.ConnectionId = connectionId;
                    userConnection.ConnectedAt = DateTime.UtcNow;
                    userConnection.isConnected = true;
                }

                await _unitOfWork.SaveChangesAsync();


                UserDto? userProfile = await _unitOfWork.user.GetUser(userId);
                
                OnlineUserInfo onlineUserInfo = new ()
                {
                    UserId = userId,
                    ConnectionId = connectionId,
                    UserName = userProfile?.FullName ?? "Unknown",
                    ConnectedAt = DateTime.UtcNow
                };
                
                _onlineUsers.AddOrUpdate(userId, onlineUserInfo, (key, old) => onlineUserInfo);

                await JoinUserConversationsAsync(userGuid, connectionId);

                await Clients.Others.SendAsync("UserConnected", new
                {
                    userId = userGuid,
                    userName = onlineUserInfo.UserName,
                    connectedAt = onlineUserInfo.ConnectedAt
                });

                await Clients.Caller.SendAsync("OnlineUsersList", GetOnlineUsersList());

                await Clients.All.SendAsync("OnlineUsersCount", _onlineUsers.Count);

                await NotifyUnreadCount(userGuid);

                _logger.LogInformation("✅ User connected: {UserId} ({UserName}) - ConnectionId: {ConnectionId}", 
                    userId, onlineUserInfo.UserName, connectionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error in OnConnectedAsync");
            }

            await base.OnConnectedAsync();
        }


        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var userId = Context.UserIdentifier;
                
                if (string.IsNullOrEmpty(userId))
                {
                    await base.OnDisconnectedAsync(exception);
                    return;
                }

                Guid userGuid = Guid.Parse(userId);

                var userConnection = await _unitOfWork.userConnection.FindAsync(x => x.UserId == userGuid);
                if (userConnection != null)
                {
                    userConnection.isConnected = false;
                    userConnection.ConnectedAt = DateTime.UtcNow;
                    await _unitOfWork.SaveChangesAsync();
                }

                _onlineUsers.TryRemove(userId, out var removedUser);

                await Clients.Others.SendAsync("UserDisconnected", new
                {
                    userId = userGuid,
                    userName = removedUser?.UserName ?? "Unknown",
                    disconnectedAt = DateTime.UtcNow
                });

                await Clients.All.SendAsync("OnlineUsersCount", _onlineUsers.Count);

                _logger.LogInformation("👋 User disconnected: {UserId} - Reason: {Reason}", 
                    userId, exception?.Message ?? "Normal disconnect");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error in OnDisconnectedAsync");
            }

            await base.OnDisconnectedAsync(exception);
        }



  
        public async Task SendMessage(Guid receiverId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                _logger.LogWarning("⚠️ Attempt to send empty message");
                await Clients.Caller.SendAsync("Error", new { message = "Message cannot be empty" });
                return;
            }

            if (receiverId == Guid.Empty)
            {
                _logger.LogError("❌ Invalid receiver ID");
                await Clients.Caller.SendAsync("Error", new { message = "Invalid receiver ID" });
                return;
            }

            Guid senderId = Guid.Parse(Context.UserIdentifier!);

            await _unitOfWork.BeginTransactionAsync();
            
            try
            {
                var conversation = await _unitOfWork.Coversation
                    .GetConversation(senderId.ToString(), receiverId.ToString());

                if (conversation == null)
                {
                    UserDto? receiverUser = await _unitOfWork.user.GetUser(receiverId.ToString());
                    
                    conversation = new Conversation
                    {
                        Title = $"{receiverUser?.FullName ?? "User"}",
                        Participants = new List<Participant>
                        {
                            new Participant { UserId = senderId.ToString() },
                            new Participant { UserId = receiverId.ToString() }
                        }
                    };
                    
                    conversation.Id = await _unitOfWork.Coversation.AddAsync(conversation);
                    await _unitOfWork.SaveChangesAsync();

                    _logger.LogInformation("💬 New conversation created: {ConversationId}", conversation.Id);
                }

                Message messageEntity = new ()
                {
                    ConversationId = conversation.Id,
                    Content = message,
                    CreateAt = DateTime.UtcNow,
                    IsRead = false,
                    receiverId = receiverId.ToString(),
                    senderId = senderId.ToString()
                };

                await _unitOfWork.Message.AddAsync(messageEntity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                await Groups.AddToGroupAsync(Context.ConnectionId, conversation.Id.ToString());

                UserConnection? receiverConnection = await _unitOfWork.userConnection.FindAsync(x => x.UserId == receiverId);
                if (receiverConnection != null && receiverConnection.isConnected)
                {
                    await Groups.AddToGroupAsync(receiverConnection.ConnectionId, conversation.Id.ToString());
                }

                UserDto? senderInfo = await _unitOfWork.user.GetUser(senderId.ToString());

                var messageData = new
                {
                    messageId = messageEntity.Id,
                    conversationId = conversation.Id,
                    senderId = senderId,
                    senderName = senderInfo?.FullName ?? "Unknown",
                    receiverId = receiverId,
                    content = message,
                    timestamp = messageEntity.CreateAt,
                    isRead = false
                };

                await Clients.Group(conversation.Id.ToString())
                    .SendAsync("ReceiveMessage", messageData);

                if (receiverConnection != null && receiverConnection.isConnected)
                {
                    await Clients.Client(receiverConnection.ConnectionId)
                        .SendAsync("NewMessageNotification", new
                        {
                            title = $"New message from {senderInfo?.FullName ?? "Unknown"}",
                            message = message.Length > 50 ? message.Substring(0, 50) + "..." : message,
                            senderId = senderId,
                            senderName = senderInfo?.FullName ?? "Unknown",
                            conversationId = conversation.Id,
                            timestamp = DateTime.UtcNow
                        });

                    await NotifyUnreadCount(receiverId);
                }

                _logger.LogInformation("💬 Message sent from {SenderId} to {ReceiverId} in conversation {ConversationId}", 
                    senderId, receiverId, conversation.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error sending message from {SenderId} to {ReceiverId}", 
                    senderId, receiverId);
                
                await _unitOfWork.RollbackTransactionAsync();
                
                await Clients.Caller.SendAsync("Error", new 
                { 
                    message = "Failed to send message. Please try again.",
                    error = ex.Message 
                });
            }
        }

        public async Task MarkMessagesAsRead(Guid conversationId)
        {
            try
            {
                string userId = Context.UserIdentifier!;
                
                bool isParticipant = await _unitOfWork.participantRepo
                    .IsParticipantInConversation(conversationId, userId);
                
                if (!isParticipant)
                {
                    _logger.LogWarning("⚠️ Unauthorized attempt by {UserId} to mark messages as read in {ConversationId}", 
                        userId, conversationId);
                    return;
                }

                await _unitOfWork.Message.MarkMessagesAsRead(conversationId, userId);
                
                await _unitOfWork.SaveChangesAsync();

                await Clients.Group(conversationId.ToString())
                    .SendAsync("MessagesMarkedAsRead", new
                    {
                        conversationId = conversationId,
                        userId = userId,
                        timestamp = DateTime.UtcNow
                    });

                await NotifyUnreadCount(Guid.Parse(userId));

                _logger.LogInformation("✅ Messages marked as read in conversation {ConversationId} by {UserId}", 
                    conversationId, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error marking messages as read");
            }
        }


        public async Task UserTyping(Guid conversationId, Guid receiverId)
        {
            try
            {
                var userId = Context.UserIdentifier!;
                var userInfo = await _unitOfWork.user.GetUser(userId);

                var receiverConnection = await _unitOfWork.userConnection.FindAsync(x => x.UserId == receiverId);
                
                if (receiverConnection != null && receiverConnection.isConnected)
                {
                    await Clients.Client(receiverConnection.ConnectionId)
                        .SendAsync("UserTyping", new
                        {
                            conversationId = conversationId,
                            userId = userId,
                            userName = userInfo?.FullName ?? "Unknown"
                        });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error in UserTyping");
            }
        }

        public async Task UserStoppedTyping(Guid conversationId, Guid receiverId)
        {
            try
            {
                var userId = Context.UserIdentifier!;

                var receiverConnection = await _unitOfWork.userConnection.FindAsync(x => x.UserId == receiverId);
                
                if (receiverConnection != null && receiverConnection.isConnected)
                {
                    await Clients.Client(receiverConnection.ConnectionId)
                        .SendAsync("UserStoppedTyping", new
                        {
                            conversationId = conversationId,
                            userId = userId
                        });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error in UserStoppedTyping");
            }
        }


        public async Task<object> GetOnlineUsers()
        {
            try
            {
                var onlineUsers = GetOnlineUsersList();
                await Clients.Caller.SendAsync("OnlineUsersList", onlineUsers);
                return onlineUsers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error getting online users");
                return new { users = new List<object>(), count = 0 };
            }
        }

        public async Task<bool> IsUserOnline(string userId)
        {
            var isOnline = _onlineUsers.ContainsKey(userId);
            
            await Clients.Caller.SendAsync("UserOnlineStatus", new
            {
                userId = userId,
                isOnline = isOnline
            });

            return isOnline;
        }


        private async Task JoinUserConversationsAsync(Guid userId, string connectionId)
        {
            try
            {
                var conversationIds = await _unitOfWork.Coversation.GetConversationID(userId.ToString());
                
                if (conversationIds == null || !conversationIds.Any())
                {
                    _logger.LogInformation("ℹ️ User {UserId} has no conversations yet", userId);
                    return;
                }

                foreach (var conversationId in conversationIds)
                {
                    await Groups.AddToGroupAsync(connectionId, conversationId.ToString());
                }

                _logger.LogInformation("✅ User {UserId} joined {Count} conversation groups", 
                    userId, conversationIds.Count());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error joining user conversations");
            }
        }

        private async Task NotifyUnreadCount(Guid userId)
        {
            try
            {
                var connection = await _unitOfWork.userConnection.FindAsync(x => x.UserId == userId);
                
                if (connection == null || !connection.isConnected) 
                    return;

                var count = await _unitOfWork.Message.GetTotalUnreadMessages(userId);

                await Clients.Client(connection.ConnectionId)
                    .SendAsync("UpdateUnreadCount", new
                    {
                        count = count,
                        timestamp = DateTime.UtcNow
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error notifying unread count");
            }
        }

        private object GetOnlineUsersList()
        {
            var currentUserId = Context.UserIdentifier;
            
            var users = _onlineUsers.Values
                .Where(u => u.UserId != currentUserId) 
                .Select(u => new
                {
                    userId = u.UserId,
                    userName = u.UserName,
                    connectedAt = u.ConnectedAt,
                    isOnline = true
                })
                .OrderBy(u => u.userName)
                .ToList();

            return new
            {
                users = users,
                count = users.Count,
                timestamp = DateTime.UtcNow
            };
        }

    }


}