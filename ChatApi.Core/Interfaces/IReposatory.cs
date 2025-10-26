using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ChatApi.Core.Common;

namespace ChatApi.Core.Interfaces
{
    public interface IReposatory<T,E> where T : class , IEntity<E>
    {
        Task<PagedResult<T>> GetAllAsync(int pageNumber, int pageSize,Expression<Func<T, bool>> predicate);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
        Task<E> AddAsync(T Entity);
        void DeleteAsync(T Entity);
        Task<bool> UpdateAsync(T Entity);
    }
}