using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.Person;
using FamilyTreeBackend.Core.Domain.Constants;
﻿using AutoMapper;
using FamilyTreeBackend.Core.Application.Models.PersonModels;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Enums;
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
        private readonly IMapper _mapper;

        public PersonService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<PersonService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PersonDTO> AddNewParent(string userPerformingCreation, AddNewParentToPersonModel input)
        {
            await using var transaction = await _unitOfWork.CreateTransaction();
            try
            {
                // check person is valid 
                var operatingPerson = await _unitOfWork.Repository<Person>().GetDbset().FirstOrDefaultAsync(e => e.Id == input.PersonId);
                if (operatingPerson == null)
                {
                    throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedPersonFromId);
                }

                // check if user connected to this new parent node is an existing tree node or not (we dont want the user to exist as a tree node)
                if (input.ParentInfo.UserId != null)
                {
                    var connectedUser = await _userManager.FindByIdAsync(input.ParentInfo.UserId);
                    if (connectedUser == null)
                    {
                        throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedUserFromId);
                    }
                    var existingNodeRelatedToUser = await _unitOfWork.Repository<Person>().GetDbset().AnyAsync(e => e.FamilyTreeId == operatingPerson.FamilyTreeId && e.UserId == input.ParentInfo.UserId);
                    if (existingNodeRelatedToUser)
                    {
                        throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_UserAlreadyExistedInTree);
                    }
                }

                // Do person creation
                var newPersonValues = input.ParentInfo;
                var newParent = new Person()
                {
                    FirstName = newPersonValues.FirstName,
                    LastName = newPersonValues.LastName,
                    DateOfBirth = newPersonValues.DateOfBirth,
                    DateOfDeath = newPersonValues.DateOfDeath,
                    Gender = newPersonValues.Gender,
                    Note = newPersonValues.Note,
                    UserId = newPersonValues.UserId,
                    FamilyTreeId = operatingPerson.FamilyTreeId
                };
                newParent = await _unitOfWork.Repository<Person>().AddAsync(newParent);
                await _unitOfWork.SaveChangesAsync();

                // Lastly, check if the operating person is in any family
                var connectedFamily = await _unitOfWork.Repository<Family>().GetDbset().FirstOrDefaultAsync(e => e.Id == operatingPerson.ChildOf);
                if(connectedFamily != null)
                {
                    // check for an empty slot to insert in a parent if a family is found
                    if (connectedFamily.Parent1Id == null)
                    {
                        connectedFamily.Parent1Id = newParent.Id;
                    }
                    else if (connectedFamily.Parent2Id == null)
                    {
                        connectedFamily.Parent2Id = newParent.Id;
                    }
                    else
                    {
                        throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_NoSlotForParentOfPerson);
                    }
                    _unitOfWork.Repository<Family>().Update(connectedFamily);
                }
                else
                {
                    // if there is no previously existing family, we create a new family
                    var newRelationship = new Relationship()
                    {
                        RelationshipType = RelationshipType.UNKNOWN,
                    };
                    var newFamily = new Family()
                    {
                        Parent1Id = newParent.Id,
                        FamilyTreeId = operatingPerson.FamilyTreeId,
                        Relationship = newRelationship,
                    };
                    newFamily = await _unitOfWork.Repository<Family>().AddAsync(newFamily);
                    operatingPerson.ChildOfFamily = newFamily;
                    _unitOfWork.Repository<Person>().Update(operatingPerson);
                }
                await _unitOfWork.SaveChangesAsync();

                // Entry and populate fields
                var entry = _unitOfWork.Entry(operatingPerson);
                if (entry != null)
                {
                    await entry.Reference(e => e.FamilyTree).LoadAsync();
                    await entry.Reference(e => e.ChildOfFamily).LoadAsync();
                    await entry.Reference(e => e.ConnectedUser).LoadAsync();

                    if (operatingPerson.ChildOfFamily != null)
                    {
                        var entryFamily = _unitOfWork.Entry(operatingPerson.ChildOfFamily);
                        await entryFamily.Reference(e => e.Parent1).LoadAsync();
                        await entryFamily.Reference(e => e.Parent2).LoadAsync();
                        await entryFamily.Reference(e => e.Relationship).LoadAsync();
                    }
                }
                await transaction.CommitAsync();
                return new PersonDTO(operatingPerson);
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
                _logger.LogInformation(ex, LoggingMessages.PersonService_ErrorMessage);
                throw;
            }
        }

        public async Task<PersonDTO> AddNewSpouse(string userPerformingCreation, AddNewSpouseToPersonModel input)
        {
            return null;
        }

        public async Task<PersonDTO> AddNewChild(string userPerformingCreation, AddNewChildToPersonModel input)
        {
            return null;
        }

        public async Task<PersonModel> GetPerson(long id)
        {
            Person person =  await _unitOfWork.Repository<Person>().GetDbset()
                .Include(p => p.ChildOfFamily)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync<Person>();

            if (person == null)
            {
                throw new PersonNotFoundException($"Person not found: {id}");
            }

            var personModel = _mapper.Map<Person, PersonModel>(person);

            return personModel;
        }

        public async Task<IEnumerable<PersonModel>> GetPersonChildren(long id)
        {
            List<PersonModel> result = new List<PersonModel>();

            IEnumerable<Person> people = await _unitOfWork.Repository<Person>().GetDbset()
                .Where(p => p.ChildOfFamily.Parent1Id == id || p.ChildOfFamily.Parent2Id == id)
                .Include(p => p.ChildOfFamily)
                .ToListAsync();

            foreach(var person in people)
            {
                PersonModel personModel = _mapper.Map<PersonModel>(person);
                result.Add(personModel);
            }

            return result;
        }

        public async Task RemovePerson(long id)
        {
            bool anyChildren =  await _unitOfWork.Repository<Person>().GetDbset()
                .Where(p => p.ChildOfFamily.Parent1Id == id || p.ChildOfFamily.Parent2Id == id)
                .AnyAsync();

            if (anyChildren)
            {
                throw new PersonHasChildrenException($"This person still have children: {id}");
            }
            Person deletedPerson = await _unitOfWork.Repository<Person>().DeleteAsync(id);
            return;
        }
    }
}
