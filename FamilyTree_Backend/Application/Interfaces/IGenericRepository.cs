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
        Task<TEntity> FindAsync(int id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> DeleteAsync(int id);

        TEntity Delete(TEntity entity);
        TEntity Update(TEntity entity);

    }
}
