using ChatApi.Application.Contract.Common;
using ChatApi.Application.Contract.Conversations.req;
using ChatApi.Application.Contract.Conversations.res;
using ChatApi.Application.Interfaces;
using ChatApi.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ChatApi.Application.Services
{
    public class ConversationService(IConversation _conversation) : IConversationService
    {
        public async Task< GeneralResponse<bool> > ChangeConversationName(ConversationReq conv)
        {
            if (conv == null || string.IsNullOrEmpty(conv.NewName) || string.IsNullOrEmpty(conv.ConversationId))
                return GeneralResponse<bool>.Failure("Invalid Data", 400);

            var conversation = await _conversation.FindAsync(x => x.Id == Guid.Parse(conv.ConversationId));
            if (conversation == null)
                return GeneralResponse<bool>.Failure("Conversation Not Found", 404);

            conversation.Title = conv.NewName;
            await _conversation.SaveAsync();
                return GeneralResponse<bool>.Success(true,"Title Changed successfully", StatusCodes.Status200OK);

        }

      

        public async Task<GeneralResponse<IEnumerable<ConversationRes>?>> GetAllConversations(string UserID)
        {
            if (string.IsNullOrEmpty(UserID))
                return GeneralResponse<IEnumerable<ConversationRes>?>.Failure("unauthorized", StatusCodes.Status401Unauthorized);

            var conversations = await _conversation.GetUserConversations(UserID);

            if (conversations == null || !conversations.Any())
                return GeneralResponse<IEnumerable<ConversationRes>?>.Failure("Conversations not found", StatusCodes.Status404NotFound);

          

            return GeneralResponse<IEnumerable<ConversationRes>?>.Success(conversations.Select(x => new ConversationRes{Id = x.Id.ToString() , Title = x.Title}), "Conversations found", StatusCodes.Status200OK);
        } }

      
    
}