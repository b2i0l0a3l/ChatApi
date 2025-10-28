using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Interfaces;

namespace ChatApi.Core.Entities
{
    public class UserConnection : IEntity<int>
    {
          public int Id { get; set; }
         public Guid UserId { get; set; }
        public string ConnectionId { get; set; } = string.Empty;
        public bool isConnected { get; set; } = true;
        public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;

    }
}