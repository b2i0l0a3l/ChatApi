using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;
using ChatApi.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApi.Infrastructure.presistence.conf
{
    public class ParticipantConfiguratioin : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.HasOne<ApplicationUser>().WithMany(x => x.Participants).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(x=> x.conversation).WithMany(c=>c.Participants).HasForeignKey(p=>p.Id).OnDelete(DeleteBehavior.Cascade);
        }
    }
}