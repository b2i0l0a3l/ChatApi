using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Application.Contract.Conversations.req
{
    public record ConversationReq
    {
        [Required]
        public string? ConversationId { get; set; }
        [Required]
        public string? NewName{ get; set; }
    }
}