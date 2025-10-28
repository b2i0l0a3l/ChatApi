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
        

        public async Task<IEnumerable<Guid>> GetConversation(string UserId)
        {
           return await  _dbSet.AsQueryable().Where(u => u.UserId == UserId).Select(p => p.conversationId).ToListAsync();

        }
    }
}