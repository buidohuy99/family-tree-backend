using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Persistence.Repository
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity> 
        where TEntity : BaseEntity
        where TContext : DbContext
    {
        private readonly TContext _context;
        private DbSet<TEntity> dbSet;

        public GenericRepository(TContext context)
        {
            _context = context;
            dbSet = _context.Set<TEntity>() ?? throw new ArgumentNullException(nameof(context));
        }

        public DbSet<TEntity> GetDbset()
        {
            return _context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<TEntity> DeleteAsync(int id)
        {
            var entity = await dbSet.FindAsync(id);

            if (entity == null) {
                return null;
            }

            dbSet.Remove(entity);

            return entity;
        }

        public async Task<TEntity> FindAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public Task<List<TEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        
        public TEntity Update(TEntity entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public TEntity Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);

            return entity;
        }
    }
}
