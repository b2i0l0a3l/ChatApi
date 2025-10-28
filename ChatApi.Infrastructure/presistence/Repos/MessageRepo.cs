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
            =>_logger = logger;
        public async Task<int> GetUnreadCount(Guid conversationId, string userId)
        {
            try
            {
                var r = await _dbSet.CountAsync(m => m.ConversationId == conversationId && m.senderId != userId && !m.IsRead);
                _logger.LogInformation("Retrieved unread message count");
                return r;
                
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting unread message count");
                return -1;
            }
            
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
                    .ToListAsync();
                _logger.LogInformation("Retrieved messages for conversation");
                return r;
                
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving messages for conversation");
                return Enumerable.Empty<Message>();
            }
        }  

    }
}