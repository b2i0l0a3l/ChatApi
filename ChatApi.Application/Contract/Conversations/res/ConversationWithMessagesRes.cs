using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.MessageContract.res;

namespace ChatApi.Application.Contract.Conversations.res
{
    public class ConversationWithMessagesRes
    {
        public string? Title { get; set; }
        public IEnumerable<MessageRes>? messages { get; set; }
    }
}