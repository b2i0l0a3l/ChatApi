using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;

namespace ChatApi.Core.Interfaces
{
    public interface IConversation : IReposatory<Conversation , Guid>
    {
            Task<IEnumerable<Conversation>> GetUserConversations(string userId);

    }
}