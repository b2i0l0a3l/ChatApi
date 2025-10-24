using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApi.Application.Contract.Auth.Req;
using ChatApi.Application.Contract.Auth.Res;
using ChatApi.Application.Contract.Common;
using ChatApi.Application.Contract.Token.req;
using ChatApi.Application.Interfaces;
using ChatApi.Application.Interfaces.Auth;
using ChatApi.Infrastructure.Identity;
using ChatApi.Infrastructure.presistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SQLitePCL;

namespace ChatApi.Application.Services.authService
{
    public class AuthService : IAuth
    {
        private readonly ILogin _LoginService;
        private readonly IRegister _register;
        private readonly IRefresh _refresh;
        
        public AuthService(ILogin loginService, IRegister register, IRefresh refresh)
        {
            _LoginService = loginService;
            _register = register;
            _refresh = refresh;
        }


        public async Task<GeneralResponse<AuthRes>> Login(LoginModel model) =>
             await _LoginService.Login(model);
        public async Task<GeneralResponse<string>> Register(SingUp model)
            => await _register.Register(model);

        public async Task<GeneralResponse<TokenReq>> Refresh(TokenReq model)
        => await _refresh.Refresh(model);
    }
}