using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;
using ChatApi.Core.Interfaces;

namespace ChatApi.Infrastructure.presistence.Repos
{
    public class ConversationRepo : Repository<Conversation, Guid>, IConversation
    {
        public ConversationRepo(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Conversation>> GetUserConversations(string userId)
        =>  new List<Conversation>();

    }
}