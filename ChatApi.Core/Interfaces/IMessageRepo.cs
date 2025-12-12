using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;

namespace ChatApi.Core.Interfaces
{
    public interface IMessageRepo : IReposatory<Message , int>
    {
        Task<List<Message>?> GetUnreadMessagesForConversation(Guid conversationId, string userId);
        Task<IEnumerable<Message>?> GetMessagesForConversation(Guid conversationId,string UserId, int page, int pageSize);
        Task<int?> GetTotalUnreadMessages(Guid UserID);
        Task MarkMessagesAsRead(Guid conversationId, string userId);
        Task<IEnumerable<Message>?> SearchMessages(string query, Guid? conversationId, string userId);

    }
}