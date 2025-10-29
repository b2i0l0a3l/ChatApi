using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ChatApi.Core.Common;
using ChatApi.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Infrastructure.presistence.Repos
{
    public class Repository<T,E> : IReposatory<T,E> where T : class, IEntity<E>
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<E> AddAsync(T Entity)
        {
            await _dbSet.AddAsync(Entity);
            return Entity.Id;
        }

        public void DeleteAsync(T Entity)
        {
            _dbSet.Remove(Entity);
        }

        public async Task<PagedResult<T>> GetAllAsync(int pageNumber, int pageSize,Expression<Func<T, bool>> predicate = null)
        {
            var query = _dbSet.AsQueryable();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            var TotoalItems = await _dbSet.CountAsync();
            
            var items = await query
            .Skip((pageNumber - 1) * pageSize).
            Take(pageSize).ToListAsync();
            return new PagedResult<T>
            {
                Items = items,
                TotalItems = TotoalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.FirstOrDefaultAsync(predicate);
        

        public async Task<bool> UpdateAsync(T Entity)
        {
            var existing = await _dbSet.FindAsync(Entity.Id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(Entity);
            return true;
        }
    }
}