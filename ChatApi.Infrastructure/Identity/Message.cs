using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Infrastructure.Identity
{
    public class Message
    {
        public int Id { get; set; }
        public string? senderId { get; set; }
        public string? receiverId { get; set; }
        public string? Content { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsRead { get; set; }

        public ApplicationUser? Sender { get; set; }
        public ApplicationUser? Receiver { get; set; }
    }
}