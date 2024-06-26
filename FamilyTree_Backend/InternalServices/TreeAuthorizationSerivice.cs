﻿using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
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
                .SingleOrDefault(tr => tr.Id == treeId);
            if(tree == null)
            {
                throw new TreeNotFoundException(TreeExceptionMessages.TreeNotFound, treeId);
            }

            var entry = _unitOfWork.Entry(tree);
            entry.Collection(tr => tr.Editors).Load();
            entry.Collection(tr => tr.People).Load();

            var result = _authorizationService.AuthorizeAsync(user, tree, requirements);
            return result;
        }

        public Task<AuthorizationResult> AuthorizeWithPersonAsync(ClaimsPrincipal user, long personId, IAuthorizationRequirement requirements)
        {
            var person = _unitOfWork.Repository<Person>().GetDbset().Find(personId);
            if (person == null)
            {
                throw new PersonNotFoundException(PersonExceptionMessages.PersonNotFound, personId);
            }

            LoadTreeToPerson(person);
            
            var result = _authorizationService.AuthorizeAsync(user, person, requirements);
            return result;
        }

        public Task<AuthorizationResult> AuthorizeWithEventAsync(ClaimsPrincipal user, long eventId, IAuthorizationRequirement requirements)
        {
            var eventItem = _unitOfWork.Repository<FamilyEvent>().GetDbset().Find(eventId);
            if (eventItem == null)
            {
                throw new PersonNotFoundException(CalendarExceptionMessages.FamilyEventNotFound, eventId);
            }

            var entry = _unitOfWork.Entry(eventItem);
            entry.Reference(p => p.FamilyTree).Load();

            FamilyTree tree = eventItem.FamilyTree;
            var treeEntry = _unitOfWork.Entry(tree);
            treeEntry.Collection(tr => tr.Editors).Load();
            treeEntry.Collection(tr => tr.People).Load();

            var result = _authorizationService.AuthorizeAsync(user, tree, requirements);
            return result;
        }

        public Task<AuthorizationResult> AuthorizeWithMemoryAsync(ClaimsPrincipal user, long memoryId, IAuthorizationRequirement requirements)
        {
            var memoryItem = _unitOfWork.Repository<FamilyMemory>().GetDbset().Find(memoryId);
            if (memoryItem == null)
            {
                throw new PersonNotFoundException(MemoryExceptionMessages.FamilyMemoryNotFound, memoryId);
            }

            var entry = _unitOfWork.Entry(memoryItem);
            entry.Reference(p => p.FamilyTree).Load();

            FamilyTree tree = memoryItem.FamilyTree;
            var treeEntry = _unitOfWork.Entry(tree);
            treeEntry.Collection(tr => tr.Editors).Load();
            treeEntry.Collection(tr => tr.People).Load();

            var result = _authorizationService.AuthorizeAsync(user, tree, requirements);
            return result;
        }

        #region Private methods
        private void LoadTreeToPerson(Person person)
        {
            var entry = _unitOfWork.Entry(person);
            entry.Reference(p => p.FamilyTree).Load();

            FamilyTree tree = person.FamilyTree;
            var treeEntry = _unitOfWork.Entry(tree);
            treeEntry.Collection(tr => tr.Editors).Load();
        }
        #endregion
    }
}
