    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ChatApi.Application.Contract.Common;
    using ChatApi.Application.Contract.MessageContract.req;
    using ChatApi.Application.Contract.MessageContract.res;
    using ChatApi.Application.Interfaces;
    using ChatApi.Core.Interfaces;
    using Microsoft.AspNetCore.Http;

    namespace ChatApi.Application.Services
    {
        public class MessageService(IMessageRepo _message, ICUrrentUser _CurrentUser ) : IMessageService
    {

        public async Task<GeneralResponse<IEnumerable<MessageRes>>> GetAllMessages(MessageReq model)
        {
            if(string.IsNullOrEmpty(_CurrentUser.UserId))
                return GeneralResponse<IEnumerable<MessageRes>>.Failure("Unauthorized", 401);

            if (model == null)
                return GeneralResponse<IEnumerable<MessageRes>>.Failure("Invalid Data", 400);

            var r = await _message.GetMessagesForConversation(model.ConversationId, _CurrentUser.UserId ,model.PageNumber, model.PageSize);

            if (r == null) return GeneralResponse<IEnumerable<MessageRes>>.Failure("Messages Not Found", 500);

            return GeneralResponse<IEnumerable<MessageRes>>.Success(r.Select(x=> new MessageRes(x)), "Messages found", 200);
            }

        public async Task<GeneralResponse<int>> GetUnreadCount()
        {
            if(string.IsNullOrEmpty(_CurrentUser.UserId))
                return GeneralResponse<int>.Failure("Unauthorized", 401);


            if (!Guid.TryParse(_CurrentUser.UserId, out Guid userGuid))
                return GeneralResponse<int>.Failure("Invalid User ID format", 400);

            var count = await _message.GetTotalUnreadMessages(userGuid);

            if (count == null || count < 0)
                return GeneralResponse<int>.Failure("Error getting unread count", 500);

            return GeneralResponse<int>.Success(count.Value, "Unread count retrieved", 200);
        }

        public async Task<GeneralResponse<IEnumerable<MessageRes>>> SearchMessages(string query, Guid? conversationId)
        {
            if(string.IsNullOrEmpty(_CurrentUser.UserId))
                return GeneralResponse<IEnumerable<MessageRes>>.Failure("Unauthorized", 401);

            if (string.IsNullOrWhiteSpace(query))
                return GeneralResponse<IEnumerable<MessageRes>>.Failure("Search query is required", 400);

            var results = await _message.SearchMessages(query, conversationId, _CurrentUser.UserId);

            if (results == null)
                return GeneralResponse<IEnumerable<MessageRes>>.Failure("Error searching messages", 500);

            return GeneralResponse<IEnumerable<MessageRes>>.Success(
                results.Select(x => new MessageRes(x)), 
                $"Found {results.Count()} messages", 
                StatusCodes.Status200OK);
        }
        }
    }