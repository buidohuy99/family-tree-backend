using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FamilyTreeBackend.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _dbContext;
        private Dictionary<string, dynamic> repositoriesPrototypes = new Dictionary<string, dynamic>();

        public UnitOfWork(TContext context)
        {
            _dbContext = context;
            repositoriesPrototypes = new Dictionary<string, dynamic>();

        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {

            var repoType = typeof(TEntity).Name;

            if (!repositoriesPrototypes.ContainsKey(repoType))
            {
                repositoriesPrototypes.Add(repoType, new GenericRepository<TEntity, TContext>(_dbContext));
            }

            return repositoriesPrototypes[repoType];
        }
    }

}
