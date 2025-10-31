    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ChatApi.Application.Contract.Common;
    using ChatApi.Application.Contract.MessageContract.req;
    using ChatApi.Application.Contract.MessageContract.res;
    using ChatApi.Application.handler;
    using ChatApi.Application.Interfaces;
    using ChatApi.Core.Interfaces;
    using Microsoft.AspNetCore.Http;

    namespace ChatApi.Application.Services
    {
        public class MessageService(IUniteOfWork _unite ) : IMessageService
        {
            public async Task<GeneralResponse<IEnumerable<MessageRes>>> GetAllMessages(MessageReq model)
            {

            if (model == null)
                return GeneralResponse<IEnumerable<MessageRes>>.Failure("Invalid Data", StatusCodes.Status400BadRequest);
                    
                var r = await _unite.Message.GetMessagesForConversation(model.ConversationId, model.PageNumber, model.PageSize);

                if (r == null) return GeneralResponse<IEnumerable<MessageRes>>.Failure("Messages Not Found", StatusCodes.Status500InternalServerError);

                
                return GeneralResponse<IEnumerable<MessageRes>>.Success(r.Select(x => new MessageRes(x)), "Messages found", 200);
            }
        }
    }