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
    public class MessageRepo: Repository<Message, int> ,  IMessageRepo
    {
        private readonly ILogger<MessageRepo> _logger ;
        public MessageRepo(AppDbContext context, ILogger<MessageRepo> logger) : base(context)
        => _logger = logger;
            
            public async Task<List<Message>> GetUnreadMessagesForConversation(Guid conversationId, string userId)
        {
            return await _dbSet
                .Where(m => m.ConversationId == conversationId && m.receiverId == userId && !m.IsRead)
                .ToListAsync();
        }

                
        public async Task<IEnumerable<Message>> GetMessagesForConversation(Guid conversationId, int page, int pageSize)
        {
            try
            {
                var r = await _dbSet
                    .Where(m => m.ConversationId == conversationId)
                    .OrderByDescending(m => m.CreateAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .OrderBy(c =>c.CreateAt)
                    .ToListAsync();
                _logger.LogInformation("Retrieved messages for conversation");
                return r;
                
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving messages for conversation");
                return Enumerable.Empty<Message>();
            }
        }

        public async Task<int> GetTotalUnreadMessages(Guid UserID)
        {
              try
            {
                var r = await _dbSet.CountAsync(x => x.senderId == UserID.ToString() && !x.IsRead);
                _logger.LogInformation("Retrieved messages for conversation");
                return r;
                
            }catch(Exception ex)
            {
                _logger.LogError( "Error retrieving messages for conversation : {ex.Message}",ex.Message);
                return -1;
            }
        }

        public async Task MarkMessagesAsRead(Guid conversationId, string userId)
        {
                    var unread = await _dbSet
                .Where(m => m.ConversationId == conversationId && m.receiverId == userId && !m.IsRead)
                .ToListAsync();

            if (unread.Any())
                    {
                foreach (var msg in unread)
                {
                    msg.IsRead = true;
                    msg.CreateAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}