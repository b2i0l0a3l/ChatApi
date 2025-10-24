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
    public class Repository<T> : IReposatory<T> where T : class, IEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<int> AddAsync(T Entity)
        {
            await _dbSet.AddAsync(Entity);
            await _context.SaveChangesAsync();
            return Entity.Id;
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            var r = await _dbSet.FindAsync(Id);
            if (r == null)
                return false;
            _dbSet.Remove(r);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<T>> GetAllAsync(int pageNumber, int pageSize)
        {
            var TotoalItems = await _dbSet.CountAsync();
            var items = await _dbSet.Skip((pageNumber - 1) * pageSize).
            Take(pageSize).ToListAsync();
            return new PagedResult<T>
            {
                Items = items,
                TotalItems = TotoalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        
        }

        public async Task<T?> GetEntity(Expression<Func<T, bool>> predicate)
        => await _dbSet.FirstOrDefaultAsync(predicate);
        

        public async Task<bool> UpdateAsync(T Entity)
        {
            var existing = await _dbSet.FindAsync(Entity.Id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(Entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}