using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Entities.KeyLess;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Persistence.Repository
{
    public static class RepositoryExtensions
    {
        public static IQueryable<UserConnectionModel> GetUserConnection(this IGenericRepository<UserConnectionModel> repository,
            string sourceUserId, string destinationUserId)
        {
            var user1 = new SqlParameter("User1", sourceUserId);
            var user2 = new SqlParameter("User2", destinationUserId);

            IQueryable<UserConnectionModel> result = repository.GetDbset()
                .FromSqlRaw(Sql_GetUserConnection, user1, user2);

            return result;
        }

        private const string Sql_GetUserConnection = @"
with
_treesConnectedToUser1 as (
select p.FamilyTreeId
from Person p
where p.UserId = @User1
),

_treesConnectedToUser2 as (
select p.FamilyTreeId
from Person p
where p.UserId = @User2
),

_connectedPeople1 as (
select p.UserId
, p.FamilyTreeId
from Person p
where
p.UserId is not null 
and p.FamilyTreeId in (select * from _treesConnectedToUser1)
),

_connectedPeople2 as (
select p.UserId
, p.FamilyTreeId
from Person p
where 
p.UserId is not null
and p.FamilyTreeId in (select * from _treesConnectedToUser2)
)

select p1.FamilyTreeId as FamilyTreeId
, p1.UserId as SourceUserId
, p2.UserId as DestinationUserId
from _connectedPeople1 p1 inner join _connectedPeople2 p2 on p1.FamilyTreeId = p2.FamilyTreeId"; 
    }
}
