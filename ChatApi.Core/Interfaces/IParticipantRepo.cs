using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.DTO;
using ChatApi.Core.Entities;

namespace ChatApi.Core.Interfaces
{
    public interface IParticipantRepo : IReposatory<Participant, Guid >
    {
        Task<bool> IsParticipantInConversation(Guid conversationId, string userId);
        Task<List<ParticipantInfo>?> GetParticipant(Guid conversationId);
    }
}