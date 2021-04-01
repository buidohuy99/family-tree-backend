using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
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
    }
}
