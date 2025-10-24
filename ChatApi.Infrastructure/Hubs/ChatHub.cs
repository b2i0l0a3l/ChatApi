using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Infrastructure.Hubs
{
    public class ChatHub(UserManager<ApplicationUser> userManager, ApplicationDbContext context) : Hub
    {
        public static readonly ConcurrentDictionary<string, string> ConnectedUsers = new();
        
    }
}