using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;
using ChatApi.Core.Interfaces;

namespace ChatApi.Infrastructure.presistence.Repos
{
    public class UnitOfWork : IUniteOfWork
    {
        private readonly AppDbContext _context;
        public IMessageRepo Message { get; }
        public IConversation Coversation { get; }
        public IUserConnection userConnection { get; }

        public UnitOfWork( AppDbContext context, IMessageRepo messageRepo, IConversation conversationRepo, IUserConnection userConnectionRepo)
        {
            _context = context;
            Message = messageRepo;
            Coversation = conversationRepo;
            userConnection = userConnectionRepo;
        }
        public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

        public void Dispose()
         => _context.Dispose();
    }
}