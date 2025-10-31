using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Contract.Conversations.res;
using ChatApi.Application.Contract.MessageContract.res;
using ChatApi.Application.Interfaces;
using ChatApi.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ChatApi.Application.Services
{
    public class ConversationService(IUniteOfWork _unite) : IConversationService
    {

        public async Task<GeneralResponse<IEnumerable<ConversationRes>?>> GetAllConversations(string UserID)
        {
            if (string.IsNullOrEmpty(UserID))
                return GeneralResponse<IEnumerable<ConversationRes>?>.Failure("unauthorized", StatusCodes.Status401Unauthorized);

            var conversations = await _unite.Coversation.GetUserConversations(UserID);

            if (conversations == null || !conversations.Any())
                return GeneralResponse<IEnumerable<ConversationRes>?>.Failure("Conversations not found", StatusCodes.Status404NotFound);

            var tasks = conversations.Select(async x =>
            {
                var participants = await _unite.participantRepo.GetParticipant(x.Id);

                return new ConversationRes
                {
                    Id = x.Id.ToString(),
                    Title = x.Title,
                    Participant = participants
                };
            }).ToList();

            var results = await Task.WhenAll(tasks);

            return GeneralResponse<IEnumerable<ConversationRes>?>.Success(results, "Conversations found", StatusCodes.Status200OK);
        } }

      
    
}