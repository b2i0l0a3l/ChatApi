using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;
using ChatApi.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Infrastructure.presistence.Repos
{
    public class MessageRepo: Repository<Message, int> ,  IMessageRepo
    {
            public MessageRepo(AppDbContext context) : base(context)
            {
            }
        public async Task<int> GetUnreadCount(Guid conversationId, string userId)
        => await _dbSet.CountAsync(m => m.ConversationId == conversationId && m.senderId != userId && !m.IsRead);

        public async Task<IEnumerable<Message>> GetMessagesForConversation(Guid conversationId, int page, int pageSize)
            =>  await _dbSet
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.CreateAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    }
}