using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;
using ChatApi.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace ChatApi.Infrastructure.presistence.Repos
{
    public class UnitOfWork : IUniteOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;
        public IMessageRepo Message { get; }
        public IConversation Coversation { get; }
        public IUserConnection userConnection { get; }
        public IParticipantRepo participantRepo { get; }
        public IUserRepo user{ get; }
        public UnitOfWork(AppDbContext context, IMessageRepo messageRepo, IConversation conversationRepo,
         IUserConnection userConnectionRepo, IParticipantRepo participant, IUserRepo user)
        {
            _context = context;
            Message = messageRepo;
            Coversation = conversationRepo;
            userConnection = userConnectionRepo;
            participantRepo = participant;
            this.user = user;

        }
        public async Task BeginTransactionAsync()
        {
            _transaction ??= await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

        public void Dispose()
        { _context.Dispose(); _transaction?.Dispose(); }
    }
}