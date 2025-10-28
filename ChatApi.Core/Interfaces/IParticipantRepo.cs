using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;

namespace ChatApi.Core.Interfaces
{
    public interface IParticipantRepo : IReposatory<Participant, Guid >
    {
        Task<IEnumerable<Guid>> GetConversation(string UserId);
    }
}