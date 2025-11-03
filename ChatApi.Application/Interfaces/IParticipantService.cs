using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Contract.Participant.res;
using ChatApi.Infrastructure.presistence.Repos;

namespace ChatApi.Application.Interfaces
{
    public interface IParticipantService
    {
        Task<GeneralResponse<ParticipantRes>> GetParticipantByConversationID(string ConversationId);
    }
}