using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Interfaces;
using ChatApi.Infrastructure.presistence.Repos;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApi.Infrastructure.InfrastructureRegistration
{
    public static class InfrastructureRegistration 
    {
        public static void AddInfrastructureRegistration(this IServiceCollection services)
        {
            services.AddScoped(typeof(IReposatory<,>), typeof(Repository<,>));
            services.AddScoped<IUniteOfWork, UnitOfWork>();
            services.AddScoped<IConversation, ConversationRepo>();
            services.AddScoped<IMessageRepo, MessageRepo>();
            services.AddScoped<IUserConnection, UserConnectionRepo>();
        }
    }
}