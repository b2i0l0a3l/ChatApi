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
    public class MessageCofiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasOne(x => x.Conversation).WithMany(c => c.Messages).HasForeignKey(x => x.ConversationId).OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne<ApplicationUser>().WithMany().HasForeignKey(x => x.receiverId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<ApplicationUser>().WithMany().HasForeignKey(x => x.senderId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}