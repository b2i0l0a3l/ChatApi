using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;

namespace ChatApi.Core.Interfaces
{
    public interface IParticipantRepo : IReposatory<Participant, Guid >
    {
        Task<bool> IsParticipantInConversation(Guid conversationId, string userId);
        Task<IEnumerable<Guid>> GetConversation(string UserId);
        Task<IEnumerable<Conversation>?> GetConversations(string UserId);
    }
}