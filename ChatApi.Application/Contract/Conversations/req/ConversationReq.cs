using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Application.Contract.Conversations.req
{
    public record ConversationReq
    {
        public Guid ConversationId { get; set; }
        public string? UserID { get; set; }
        public int PageNumber { get; set; }
        public int pageSize { get; set; }
    }
}