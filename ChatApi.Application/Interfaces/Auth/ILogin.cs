using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Auth.Res;
using ChatApi.Application.Contract.Common;

namespace ChatApi.Application.Interfaces.Auth
{
    public interface ILogin
    {
        Task<GeneralResponse<AuthRes>> Login(LoginModel model);

        
    }
}