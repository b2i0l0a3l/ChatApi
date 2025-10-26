using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Interfaces;

namespace ChatApi.Core.Entities
{
    public class Participant : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = default!;
        public bool IsAdmin { get; set; } = false;
        public DateTimeOffset JoinedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? LastReadAt { get; set; }

        public Conversation? conversation { get; set; } 

    }
}