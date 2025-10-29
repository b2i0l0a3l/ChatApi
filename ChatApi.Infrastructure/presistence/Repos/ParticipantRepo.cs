using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;
using ChatApi.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Infrastructure.presistence.Repos
{
    public class ParticipantRepo : Repository<Participant,Guid>, IParticipantRepo
    {
        public ParticipantRepo(AppDbContext context) : base(context) { }

        public async Task<bool> IsParticipantInConversation(Guid conversationId, string userId)
        {
            return await _context.Participants
                .AnyAsync(p => p.conversationId == conversationId && p.UserId == userId);
}
        public async Task<IEnumerable<Guid>> GetConversation(string UserId)
        {
            return await _dbSet.AsQueryable().Where(u => u.UserId == UserId).Select(p => p.conversationId).ToListAsync();
        }
        public async Task<IEnumerable<Conversation>?> GetConversations(string UserId)
        {
            return await _dbSet
            .Where(x=> x.UserId == UserId)
            .Select(p => p.conversation!)
            .Distinct()
            .ToListAsync();
            
        }

        
    }
}