using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ChatApi.Core.Common;

namespace ChatApi.Core.Interfaces
{
    public interface IReposatory<T> where T : class , IEntity
    {
        Task<PagedResult<T>> GetAllAsync(int pageNumber, int pageSize);
        Task<T?> GetEntity(Expression<Func<T, bool>> predicate);
        Task<int> AddAsync(T Entity);
        Task<bool> DeleteAsync(int Id);
        Task<bool> UpdateAsync(T Entity);
    }
}