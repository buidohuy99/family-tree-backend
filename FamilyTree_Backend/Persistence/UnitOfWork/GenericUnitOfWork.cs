using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Infrastructure.Persistence.Context;
using FamilyTreeBackend.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Persistence.UnitOfWork
{
    public class GenericUnitOfWork<TContext> : IUnitOfWork where TContext : FamilyTreeDbContext
    {
        private readonly TContext _dbContext;
        private Dictionary<string, dynamic> repositoriesPrototypes = new Dictionary<string, dynamic>();

        public GenericUnitOfWork(TContext context)
        {
            _dbContext = context;
            repositoriesPrototypes = new Dictionary<string, dynamic>();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var repoType = typeof(TEntity).Name;

            if (!repositoriesPrototypes.ContainsKey(repoType))
            {
                repositoriesPrototypes.Add(repoType, new GenericRepository<TEntity>(_dbContext));
            }

            return (GenericRepository<TEntity>)repositoriesPrototypes[repoType];
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> CreateTransaction()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }

        public EntityEntry<T> Entry<T>(T obj) where T : BaseEntity
        {
            return _dbContext.Entry(obj);
        }

        public DbSet<RefreshToken> GetRefreshTokens()
        {
            return _dbContext.RefreshTokens;
        }
    }

}
