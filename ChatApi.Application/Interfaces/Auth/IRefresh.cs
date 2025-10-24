using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Auth.Req;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Contract.Token.req;

namespace ChatApi.Application.Interfaces.Auth
{
    public interface IRefresh
    {
        Task<GeneralResponse<TokenReq>> Refresh(TokenReq model);
        
    }
}