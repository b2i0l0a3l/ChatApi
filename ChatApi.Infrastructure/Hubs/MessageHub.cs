using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;

namespace ChatApi.Infrastructure.Hubs
{
    public class MessageHub(IUniteOfWork _UOW, ILogger<MessageHub> _logger) : Hub
    {
        public async Task MarkMessagesAsRead(Guid conversationId)
        {
            var userId = Context.UserIdentifier!;

            var isParticipant = await _UOW.participantRepo.IsParticipantInConversation(conversationId, userId);
            if (!isParticipant)
            {
                _logger.LogWarning($"Unauthorized attempt by {userId} to mark messages as read in {conversationId}");
                return;
            }

            await _UOW.Message.MarkMessagesAsRead(conversationId, userId);
            await _UOW.SaveChangesAsync();

            var unreadMessages = await _UOW.Message.GetUnreadMessagesForConversation(conversationId, userId);
            foreach (var msg in unreadMessages)
            {
                await Clients.Group(msg.ConversationId.ToString())
                             .SendAsync("MessageReadByReceiver", msg.Id, userId);
            }

            await NotifyUnreadCount(Guid.Parse(userId));
        }
 private async Task NotifyUnreadCount(Guid userId)
                {
            var connection = await _UOW.userConnection.FindAsync(x => x.UserId == userId);
            if (connection == null) return;
            var count = await _UOW.Message.GetTotalUnreadMessages(userId);

            await Clients.Client(connection.ConnectionId).SendAsync("UpdateUnreadCount", count);
        }
    }
    
}