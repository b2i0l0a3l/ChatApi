using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApi.Infrastructure.presistence.conf
{
    public class ConversationConfig : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.HasMany(x => x.Messages)
            .WithOne(x => x.Conversation).
            HasForeignKey(x => x.ConversationId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}