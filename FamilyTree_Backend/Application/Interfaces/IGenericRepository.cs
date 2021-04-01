using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IRepository
    {

    }
    public interface IGenericRepository<TEntity> : IRepository where TEntity: BaseEntity
    {
        public DbSet<TEntity> GetDbset();
        TEntity Find(long id);
        Task<TEntity> FindAsync(long id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> DeleteAsync(long id);

        TEntity Delete(TEntity entity);
        TEntity Update(TEntity entity);

    }
}
