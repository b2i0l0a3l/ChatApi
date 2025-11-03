using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.MessageContract.res;
using ChatApi.Core.DTO;
using ChatApi.Core.Entities;

namespace ChatApi.Application.Contract.Conversations.res
{
    
    public class ConversationRes
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
    }

}