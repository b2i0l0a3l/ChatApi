using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Contract.MessageContract.req;
using ChatApi.Application.Contract.MessageContract.res;

namespace ChatApi.Application.Interfaces
{
    public interface IMessageService
    {
        Task<GeneralResponse<IEnumerable<MessageRes>>> GetAllMessages(MessageReq model);
    }
}