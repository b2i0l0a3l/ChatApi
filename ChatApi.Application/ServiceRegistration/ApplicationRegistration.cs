using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Interfaces;
using ChatApi.Application.Interfaces.Auth;
using ChatApi.Application.Services;
using ChatApi.Application.Services.authService;
using ChatApi.Application.Services.AuthService.Login;
using ChatApi.Application.Services.AuthService.Refresh;
using ChatApi.Application.Services.AuthService.Register;
using ChatApi.Application.Services.TokenService;
using ChatApi.Application.Services.UploadFileService;
using ChatApi.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApi.Application.ServiceRegistration
{
    public static class ApplicationRegistration
    {
        public static void AddApplicationRegistration(this IServiceCollection services)
        {
            services.AddScoped<IToken, TokenService>();
            services.AddScoped<IUploadFile, UploadImageService>();
            services.AddScoped<ILogin, LoginService>();
            services.AddScoped<IRegister, RegisterService>();
            services.AddScoped<IRefresh, RefreshService>();
            services.AddScoped<IAuth, AuthService>();
            services.AddScoped<IConversationService,ConversationService>();
            
        }

    }
}