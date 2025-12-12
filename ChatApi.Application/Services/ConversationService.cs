using System.Security.Claims;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Contract.Conversations.req;
using ChatApi.Application.Contract.Conversations.res;
using ChatApi.Application.Interfaces;
using ChatApi.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ChatApi.Application.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IConversation _conversation;
        private readonly ICUrrentUser _CurrentUser;
        public ConversationService(IConversation Conv,ICUrrentUser CurrentUser)
        {
            _conversation = Conv;
            _CurrentUser = CurrentUser;
        }
        public async Task< GeneralResponse<bool> > ChangeConversationName(ConversationReq conv)
        {
            if(string.IsNullOrEmpty(_CurrentUser.UserId))
                return GeneralResponse<bool>.Failure("Unauthorized", 401);

            if (conv == null || string.IsNullOrEmpty(conv.NewName) || string.IsNullOrEmpty(conv.ConversationId))
                return GeneralResponse<bool>.Failure("Invalid Data", 400);

            var conversation = await _conversation.FindAsync(x => x.Id == Guid.Parse(conv.ConversationId));
            if (conversation == null)
                return GeneralResponse<bool>.Failure("Conversation Not Found", 404);

            conversation.Title = conv.NewName;
            await _conversation.SaveAsync();
                return GeneralResponse<bool>.Success(true,"Title Changed successfully", 200);

        }

      

        public async Task<GeneralResponse<IEnumerable<ConversationRes>?>> GetAllConversations()
        {
            if(string.IsNullOrEmpty(_CurrentUser.UserId))
                return GeneralResponse<IEnumerable<ConversationRes>?>.Failure("Unauthorized", 401);

            var conversations = await _conversation.GetUserConversations(_CurrentUser.UserId);

            if (conversations == null || !conversations.Any())
                return GeneralResponse<IEnumerable<ConversationRes>?>.Failure("Conversations not found", 404);

            return GeneralResponse<IEnumerable<ConversationRes>?>.Success(conversations.Select(x => new ConversationRes{Id = x.Id.ToString() , Title = x.Title}), "Conversations found", 200);
        } }

      
    
}