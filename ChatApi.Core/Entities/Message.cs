using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Interfaces;

namespace ChatApi.Core.Entities
{
    public class Message : IEntity<int>
    {
        public int Id { get; set; }
        
        public string? senderId { get; set; }


        public Guid? ConversationId { get; set; } 

        public string? receiverId { get; set; }
        public string? Content { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsRead { get; set; }

        public Conversation? Conversation { get; set; } 
    }
}