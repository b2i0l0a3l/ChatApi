using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;
using ChatApi.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChatApi.Infrastructure.presistence.Repos
{
    public class ConversationRepo : Repository<Conversation, Guid>, IConversation
    {
        private ILogger<ConversationRepo> _logger;

        public ConversationRepo(AppDbContext _context, ILogger<ConversationRepo> logger) : base(_context)
        {
            _logger = logger;
        }
        
        public async Task<Conversation?> GetConversation(string senderId, string receiverId)
        =>            await  _dbSet
            .Include(p => p.Participants)
            .Where(c =>
            c.Participants!.Any(p => p.UserId == senderId) &&  
            c.Participants!.Any(p => p.UserId == receiverId)   
            )
            .FirstOrDefaultAsync()??null; 
        
        public async Task<IEnumerable<Guid>?> GetConversationID(string UserId)
            => await _dbSet
            .Where(c => c.Participants!.Any(p => p.UserId == UserId))
            .Select(c => c.Id)   
            .ToListAsync()??null;
        public async Task<IEnumerable<Conversation>?> GetUserConversations(string userId)
        {
            try
            {
                var r = await _dbSet
            .Include(c => c.Participants)
            
            .Where(c => c.Participants.Any(p => p.UserId == userId))
            
            .ToListAsync();
                if (r == null) return null;

                return r;

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error While Getting Conversation : {ex.Message}", ex.Message);
                return null;
                
            }
        }
        
    }
}