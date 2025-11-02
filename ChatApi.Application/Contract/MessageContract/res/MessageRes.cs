using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;

namespace ChatApi.Application.Contract.MessageContract.res
{
    public record MessageRes
    {
        public int Id { get; set; }
        
        public string? SenderName{ get; set; }
        public string? ReceiverName{ get; set; }
        public Guid? ConversationId { get; set; } 

        public string? Content { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsRead { get; set; }

        public MessageRes(Message message,string senderFullName,string receiverName)
        {
            this.Id = message.Id;
            this.Content = message.Content;
            this.ConversationId = message.ConversationId;
            this.CreateAt = message.CreateAt;
            this.IsRead = message.IsRead;
            this.SenderName = senderFullName;
            this.ReceiverName = receiverName;
        }
    }
}