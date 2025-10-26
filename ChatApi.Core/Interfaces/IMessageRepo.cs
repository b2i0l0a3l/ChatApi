using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;

namespace ChatApi.Core.Interfaces
{
    public interface IMessageRepo : IReposatory<Message , int>
    {
        Task<IEnumerable<Message>> GetMessagesForConversation(Guid conversationId, int page, int pageSize);
        Task<int> GetUnreadCount(Guid conversationId,string userId);

    }
}