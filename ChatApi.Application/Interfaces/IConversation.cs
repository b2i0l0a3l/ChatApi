using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Contract.Conversations.req;
using ChatApi.Application.Contract.Conversations.res;
using ChatApi.Core.Entities;
using ChatApi.Infrastructure.presistence.Repos;

namespace ChatApi.Application.Interfaces
{
    public interface IConversationService
    {
        Task<GeneralResponse<IEnumerable<ConversationRes>?>> GetAllConversations();
        Task<GeneralResponse<bool>> ChangeConversationName(ConversationReq conv);
    }
}