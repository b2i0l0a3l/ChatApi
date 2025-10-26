using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;
using ChatApi.Core.Interfaces;

namespace ChatApi.Infrastructure.presistence.Repos
{
    public class UserConnectionRepo : Repository<UserConnection, int>, IUserConnection
    {
        public UserConnectionRepo(AppDbContext context) : base(context)
        {
        }
        
    }
}