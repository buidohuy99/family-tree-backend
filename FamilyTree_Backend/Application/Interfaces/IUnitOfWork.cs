using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        public DbSet<RefreshToken> GetRefreshTokens();
        public DbSet<UserConnection> GetUserConnections();
        public DbSet<RequestResponseLog> GetRequestResponseLogs();

        public int SaveChanges();
        
        public Task<int> SaveChangesAsync();

        public Task<IDbContextTransaction> CreateTransaction();
        public EntityEntry<T> Entry<T>(T obj) where T : BaseEntity;
    }
}
