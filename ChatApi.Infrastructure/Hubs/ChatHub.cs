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
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Infrastructure.Hubs
{
    public class ChatHub(IUniteOfWork _UOW) : Hub
    {
       public async Task SendMessage(Guid recieverID, string message)
        {

            var senderId = Guid.Parse(Context.UserIdentifier!);
            var receiverConnection = await _UOW.userConnection.FindAsync(x => x.UserId == recieverID);

            if (receiverConnection != null)
            {
                await Clients.Client(receiverConnection.ConnectionId).SendAsync("ReceiveMessage", senderId, message);
            }

            await _UOW.Message.AddAsync(new Message
            {
                Content = message,
                CreateAt = DateTime.UtcNow,
                IsRead = false,
                receiverId = recieverID.ToString(),
                senderId = senderId.ToString(),
            });
            await _UOW.SaveChangesAsync();
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            var ConnectionId = Context.ConnectionId;

            var userConnections = await _UOW.userConnection.FindAsync(x => x.UserId == Guid.Parse(userId!));

            if (userConnections == null)
            {
                var userConnection = new UserConnection
                {
                    UserId = Guid.Parse(userId!),
                    ConnectionId = ConnectionId
                };
                await _UOW.userConnection.AddAsync(userConnection);
            }
            else
            {
                userConnections.ConnectionId = ConnectionId;
                userConnections.ConnectedAt = DateTime.UtcNow;
            }
            await _UOW.SaveChangesAsync();
            await base.OnConnectedAsync();
        }
        public async Task JoinConversation(Guid conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
        }
    }
}