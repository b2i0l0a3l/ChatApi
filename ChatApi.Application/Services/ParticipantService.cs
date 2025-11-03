using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Contract.Participant.res;
using ChatApi.Application.Interfaces;
using ChatApi.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace ChatApi.Application.Services
{
    public class ParticipantService(IParticipantRepo participant) : IParticipantService
    {
        public async Task<GeneralResponse<ParticipantRes>> GetParticipantByConversationID(string ConversationId)
        {
            if (string.IsNullOrEmpty(ConversationId))
                return GeneralResponse<ParticipantRes>.Failure("Invalid Data", 400);

            var participants = await participant.GetParticipant(Guid.Parse(ConversationId));
            if (participants == null)
                return GeneralResponse<ParticipantRes>.Failure("No Participant Found", 404);

            return GeneralResponse<ParticipantRes>.Success(new ParticipantRes{participants = participants} ,"success",200);
        
        }
    }
}