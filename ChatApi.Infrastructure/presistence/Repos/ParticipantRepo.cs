using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.DTO;
using ChatApi.Core.Entities;
using ChatApi.Core.Interfaces;
using ChatApi.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Infrastructure.presistence.Repos
{
    public class ParticipantRepo : Repository<Participant,Guid>, IParticipantRepo
    {
        private readonly UserManager<ApplicationUser> _user;
        public ParticipantRepo(AppDbContext context, UserManager<ApplicationUser> user) : base(context) { _user = user; }

        public async Task<bool> IsParticipantInConversation(Guid conversationId, string userId)
        {
            return await _context.Participants
                .AnyAsync(p => p.conversationId == conversationId && p.UserId == userId);
        }
        private async Task<ParticipantInfo?> GetFullName(string UserId)
        {
            var user = await _user.FindByIdAsync(UserId.ToString());
            return new ParticipantInfo{Id = Guid.Parse(UserId) , FullName = user?.FullName ?? "Unbekannt"};
        }
            public async Task<List<ParticipantInfo>?> GetParticipant(Guid conversationId)
        {
            var users = await _dbSet
            .Where(c => c.conversationId == conversationId)
            .Select(x => x.UserId)
            .ToListAsync();
            var r = new List<ParticipantInfo>();
            foreach(var u in users)
            {
                var info = await GetFullName(u);
                if(info != null)
                    r.Add(info);
            }
            return r;
        }
       

        
    }
}