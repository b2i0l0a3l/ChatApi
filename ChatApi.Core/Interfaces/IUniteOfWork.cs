using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.Entities;

namespace ChatApi.Core.Interfaces
{
    public interface IUniteOfWork : IDisposable
    {
        public IMessageRepo Message { get; }
        public IConversation Coversation { get; }
        public IUserConnection userConnection { get; }
        Task<int> SaveChangesAsync();

    }
}