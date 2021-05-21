using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    class TreeAuthorizationSerivice : ITreeAuthorizationService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUnitOfWork _unitOfWork;

        public TreeAuthorizationSerivice(
            IAuthorizationService authorizationService,
            IUnitOfWork unitOfWork)
        {
            _authorizationService = authorizationService;
            _unitOfWork = unitOfWork;
        }
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, long treeId, IAuthorizationRequirement requirements)
        {
            var tree = _unitOfWork.Repository<FamilyTree>().GetDbset()
                .Include(tr => tr.Owner)
                .Include(tr => tr.Editors)
                .SingleOrDefault(tr => tr.Id == treeId);

            if(tree == null)
            {
                throw new TreeNotFoundException(TreeExceptionMessages.TreeNotFound, treeId);
            }

            var result = _authorizationService.AuthorizeAsync(user, tree, requirements);
            return result;
        }

        public Task<AuthorizationResult> AuthorizeWithPersonAsync(ClaimsPrincipal user, long treeId, long personId, IAuthorizationRequirement requirements)
        {
            var person = _unitOfWork.Repository<Person>().GetDbset().Find(personId);
            FamilyTree tree = null;
            if (treeId == person.FamilyTreeId)
            {
                tree = GetTreeFromPerson(person);
            }

            if (person == null)
            {
                throw new TreeNotFoundException(TreeExceptionMessages.TreeNotFound);
            }

            var result = _authorizationService.AuthorizeAsync(user, tree, requirements);
            return result;
        }

        private FamilyTree GetTreeFromPerson(Person person)
        {
            var entry = _unitOfWork.Entry(person);

            if (person.FamilyTree == null)
            {
                entry.Reference(p => p.FamilyTree);
            }

            FamilyTree tree = person.FamilyTree;

            if (tree.Owner == null || tree.Editors == null)
            {
                var treeEntry = _unitOfWork.Entry(tree);
                treeEntry.Reference(tr => tr.Owner).Load();
                treeEntry.Collection(tr => tr.Editors).Load();
            }

            return tree;

        }

    }
}
