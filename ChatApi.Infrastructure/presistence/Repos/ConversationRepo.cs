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


        public async Task<IEnumerable<Conversation>>? GetUserConversations(string userId)
        {
            try
            {
                var r = await _context.Participants.Where(x => x.UserId == userId).Include(p => p.conversation)
                .Select(p => p.conversation).ToListAsync();
                return r;
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error While Getting Conversation : {ex.Message}", ex.Message);
                return Enumerable.Empty<Conversation>();
                
            }
        }
        public async Task<Conversation?> IsUserForConverstaionExist(string senderId, string receiverId)
        {
            try
            {
                var r = await _dbSet.AsQueryable().Include(p => p.Participants).FirstOrDefaultAsync(
                    c => c.Participants.Any(p => p.UserId == senderId) &&
                            c.Participants.Any(p => p.UserId == receiverId)
                );
                return r;
                
            }catch(Exception ex)
            {
                _logger.LogInformation(ex, "error : {ex.Message}", ex.Message);
                return null;
            }
        }
    }
}