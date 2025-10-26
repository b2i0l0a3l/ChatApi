using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;
using ChatApi.Core.Interfaces;

namespace ChatApi.Core.Entities
{
    public class Conversation : IEntity<Guid>
    {
        public Guid Id { get; set; }  = Guid.NewGuid();
        public string? Title { get; set; }
        public ICollection<Message>? Messages { get; set; }

        public ICollection<Participant>? Participants { get; set; } = new List<Participant>();
    }
}