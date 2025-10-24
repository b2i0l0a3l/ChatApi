using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Auth.Req;
using ChatApi.Application.Contract.Common;

namespace ChatApi.Application.Interfaces.Auth
{
    public interface IRegister
    {
         Task<GeneralResponse<string>> Register(SingUp model);
        
    }
}