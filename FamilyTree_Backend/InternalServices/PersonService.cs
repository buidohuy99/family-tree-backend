using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.Person;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Infrastructure.Service.InternalServices.CustomException;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PersonService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public PersonService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<PersonService> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<PersonDTO> AddNewPerson(string userPerformingCreation, AddPersonToTreeModel input)
        {
            await using var transaction = await _unitOfWork.CreateTransaction();
            try
            {
                var familyTree = await _unitOfWork.Repository<FamilyTree>().GetDbset().FirstOrDefaultAsync(e => e.Id == input.FamilyTreeId);
                if(familyTree == null)
                {
                    throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedTreeFromId);
                }
                if(input.PersonInfo.UserId != null)
                {
                    var connectedUser = await _userManager.FindByIdAsync(input.PersonInfo.UserId);
                    if(connectedUser == null)
                    {
                        throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedUserFromId);
                    }
                }
                if(input.PersonInfo.ChildOf != null)
                {
                    var connectedPerson = await _unitOfWork.Repository<Person>().GetDbset().FirstOrDefaultAsync(e => e.Id == input.PersonInfo.ChildOf);
                    if(connectedPerson == null)
                    {
                        throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedPersonFromId);
                    }
                }
                // Do person creation
                var newPersonValues = input.PersonInfo;
                var newPerson = new Person()
                {
                    FirstName = newPersonValues.FirstName,
                    LastName = newPersonValues.LastName,
                    DateOfBirth = newPersonValues.DateOfBirth,
                    DateOfDeath = newPersonValues.DateOfDeath,
                    Gender = newPersonValues.Gender,
                    Note = newPersonValues.Note,
                    UserId = newPersonValues.UserId,
                    ChildOf = newPersonValues.ChildOf,
                    FamilyTreeId = familyTree.Id
                };
                newPerson = await _unitOfWork.Repository<Person>().AddAsync(newPerson);
                await _unitOfWork.SaveChangesAsync();
                // Entry and populate fields
                var entry = _unitOfWork.Entry(newPerson);
                if(entry != null)
                {
                    await entry.Reference(e => e.ConnectedUser).LoadAsync();
                }
                await transaction.CommitAsync();
                return new PersonDTO(newPerson);
            }
            catch(Exception ex)
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
                _logger.LogInformation(ex, LoggingMessages.PersonService_ErrorMessage);
                throw;
            }
        }

        public async Task AddNewParent(AddNewParentToPersonModel input)
        {
            return;
        }

        public async Task AddExistingParent(long personId, long parentId)
        {
            return;
        }

        public async Task AddNewSpouse(AddNewSpouseToPersonModel input)
        {
            return;
        }

        public async Task AddExistingSpouse(long personId, long spouseId)
        {
            return;
        }

        public async Task AddNewChild(AddNewChildToPersonModel input)
        {
            return;
        }

        public async Task AddExistingChild(long personId, long childId)
        {
            return;
        }
    }
}
