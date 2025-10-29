using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Contract.Conversations.req;
using ChatApi.Application.Contract.Conversations.res;
using ChatApi.Application.Contract.MessageContract.res;
using ChatApi.Application.Interfaces;
using ChatApi.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ChatApi.Application.Services
{
    public class ConversationService (IUniteOfWork _unite): IConversationService
    {
        
        public async Task<GeneralResponse<IEnumerable<ConversationRes>?>> GetAllConversations(ConversationReq req)
        {
            
            var r = await _unite.participantRepo.GetConversations(req.UserID);
            if (r == null) return  GeneralResponse<IEnumerable<ConversationRes>?>.Failure("converstations not Found",StatusCodes.Status404NotFound);

            var rr = r.Select(x => new ConversationRes { Title = x.Title }).ToList();
            return GeneralResponse<IEnumerable<ConversationRes>?>.Success(rr, "Conversations found", StatusCodes.Status200OK);
        }

        public async Task<GeneralResponse<ConversationWithMessagesRes>> GetConversationByID(ConversationReq conversation)
        {
            var r = await _unite.Coversation.FindAsync(x => x.Id == conversation.ConversationId);
            if (r != null && r.Messages != null && r.Messages.Count > 0)
            {
                var res = new ConversationWithMessagesRes { Title = r.Title, messages = r.Messages!.Select(x => new MessageRes(x)) };
                return GeneralResponse<ConversationWithMessagesRes>.Success(res, "Converstaion found", 200);
            }
                return GeneralResponse<ConversationWithMessagesRes>.Failure("Converstaion Not found",400);
            
        }
    }
}