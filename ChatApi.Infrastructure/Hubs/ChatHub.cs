using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ChatApi.Core.Entities;
using ChatApi.Core.Interfaces;
using ChatApi.Infrastructure.Identity;
using ChatApi.Infrastructure.presistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChatApi.Infrastructure.Hubs
{
    public class ChatHub(IUniteOfWork _UOW,ILogger<ChatHub> _logger) : Hub
    {
        
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if (userId == null)
            {
                await base.OnDisconnectedAsync(exception);
                return;
            }

            var userConnection = await _UOW.userConnection.FindAsync(x => x.UserId == Guid.Parse(userId!));
            if (userConnection != null)
            {
                userConnection.isConnected = false;
                userConnection.ConnectedAt = DateTime.UtcNow;
                await _UOW.SaveChangesAsync();

                await Clients.Others.SendAsync("UserDisconnected", Guid.Parse(userId!));
            }
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(Guid receiverId, string message)
        {
            try
            {
                var senderId = Guid.Parse(Context.UserIdentifier!);

                // 🔹 تحقق هل توجد محادثة بين المستخدمين
                var conversation = await _UOW.Coversation
                    .IsUserForConverstaionExist(senderId.ToString(), receiverId.ToString());

                if (conversation == null)
                {
                    conversation = new Conversation
                    {
                        Title = DateTime.UtcNow.ToString(),
                        Participants = new List<Participant>
                {
                    new Participant { UserId = senderId.ToString() },
                    new Participant { UserId = receiverId.ToString() }
                }
                    };
                    await _UOW.Coversation.AddAsync(conversation);
                }

                var messageEntity = new Message
                {
                    ConversationId = conversation.Id,
                    Content = message,
                    CreateAt = DateTime.UtcNow,
                    IsRead = false,
                    receiverId = receiverId.ToString(),
                    senderId = senderId.ToString()
                };
                await _UOW.Message.AddAsync(messageEntity);
                await _UOW.SaveChangesAsync();

                await Clients.Group(conversation.Id.ToString())
                    .SendAsync("ReceiveMessage", senderId, message);

                _logger.LogInformation($"💬 Message sent from {senderId} to {receiverId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error in SendMessage: {Message}", ex.Message);
            }

        }
        public override async Task OnConnectedAsync()
        {
            try
            {
                var userId = Context.UserIdentifier;
                var ConnectionId = Context.ConnectionId;

                var userConnections = await _UOW.userConnection.FindAsync(x => x.UserId == Guid.Parse(userId!));

                if (userConnections == null)
                {

                    var userConnection = new UserConnection
                    {
                        UserId = Guid.Parse(userId!),
                        ConnectionId = ConnectionId,
                        isConnected = true,
                        ConnectedAt = DateTime.UtcNow
                    };
                    await _UOW.userConnection.AddAsync(userConnection);
                }
                else
                {
                    userConnections.ConnectionId = ConnectionId;
                    userConnections.ConnectedAt = DateTime.UtcNow;
                    userConnections.isConnected = true;
                }

                await Clients.Others.SendAsync("UserConnected", Guid.Parse(userId!));
                await JoinUserConversationsAsync(Guid.Parse(userId!), ConnectionId);
                _logger.LogInformation($"User connected: {userId}");

            }
            catch (Exception ex)
            {
                await _UOW.RollbackTransactionAsync();
                _logger.LogError(ex.Message, "Error in OnConnectedAsync");
            }
            await base.OnConnectedAsync();

        }
        private async Task JoinUserConversationsAsync(Guid UserId , string ConnectionId)
        {
            List<Guid> r = (List<Guid>) await _UOW.participantRepo.GetConversation(UserId.ToString());
            foreach (var c in r)
            {
                await Groups.AddToGroupAsync(ConnectionId, c.ToString());
            }
         _logger.LogInformation($"User {UserId} joined {r.Count} conversations.");

        }
        public async Task JoinConversation(Guid conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
        }
    }
}