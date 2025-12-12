using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Core.DTO
{
    public record OnlineUserInfo
    {
        public string UserId { get; set; } = string.Empty;
        public string ConnectionId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime ConnectedAt { get; set; }
        
    }
}