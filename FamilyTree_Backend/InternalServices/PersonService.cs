using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.Person;
using FamilyTreeBackend.Core.Domain.Constants;
﻿using AutoMapper;
using FamilyTreeBackend.Core.Application.Models;
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

        public async Task<AddNewParentToPersonResponseModel> AddNewParent(string userPerformingCreation, AddNewParentToPersonModel input)
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

                // Lastly, check if the operating person is in any family
                var connectedFamily = await _unitOfWork.Repository<Family>().GetDbset().FirstOrDefaultAsync(e => e.Id == operatingPerson.ChildOf);
                Family newFamily = null;
                if(connectedFamily != null)
                {
                    // if he is already in family => cannot add parent
                    throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_FamilyAlreadyExist);
                }
                else
                {
                    // if there is no previously existing family, we create a new family
                    var newRelationship = new Relationship()
                    {
                        RelationshipType = RelationshipType.UNKNOWN,
                    };
                    var parent2 = new Person()
                    {
                        Gender = newParent.Gender == Gender.FEMALE ? Gender.MALE : Gender.FEMALE,
                        FamilyTreeId = operatingPerson.FamilyTreeId
                    };
                    newFamily = new Family()
                    {
                        Parent1 = newParent.Gender == Gender.MALE ? newParent : parent2,
                        Parent2 = newParent.Gender == Gender.FEMALE ? newParent : parent2,
                        FamilyTreeId = operatingPerson.FamilyTreeId,
                        Relationship = newRelationship,
                    };
                    newFamily = await _unitOfWork.Repository<Family>().AddAsync(newFamily);
                    operatingPerson.ChildOfFamily = newFamily;
                    _unitOfWork.Repository<Person>().Update(operatingPerson);
                }
                newParent = await _unitOfWork.Repository<Person>().AddAsync(newParent);
                await _unitOfWork.SaveChangesAsync();

                // Entry and populate fields
                var entry = _unitOfWork.Entry(newFamily);
                if (entry != null)
                {
                    await entry.Reference(e => e.FamilyTree).LoadAsync();
                    await entry.Reference(e => e.Parent1).LoadAsync();
                    await entry.Reference(e => e.Parent2).LoadAsync();
                }
                await transaction.CommitAsync();

                AddNewParentToPersonResponseModel response = new AddNewParentToPersonResponseModel()
                {
                    Father = new PersonDTO(newFamily.Parent1),
                    Mother = new PersonDTO(newFamily.Parent2)
                };

                return response;
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
            await using var transaction = await _unitOfWork.CreateTransaction();
            try
            {
                // check person is valid 
                var operatingPerson = await _unitOfWork.Repository<Person>().GetDbset().FirstOrDefaultAsync(e => e.Id == input.PersonId);
                if (operatingPerson == null)
                {
                    throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedPersonFromId);
                }

                // Check gender validity
                if (operatingPerson.Gender == input.SpouseInfo.Gender)
                {
                    throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_SpouseGenderNotValid);
                }

                // check if user connected to this new parent node is an existing tree node or not (we dont want the user to exist as a tree node)
                if (input.SpouseInfo.UserId != null)
                {
                    var connectedUser = await _userManager.FindByIdAsync(input.SpouseInfo.UserId);
                    if (connectedUser == null)
                    {
                        throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedUserFromId);
                    }
                    var existingNodeRelatedToUser = await _unitOfWork.Repository<Person>().GetDbset().AnyAsync(e => e.FamilyTreeId == operatingPerson.FamilyTreeId && e.UserId == connectedUser.Id);
                    if (existingNodeRelatedToUser)
                    {
                        throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_UserAlreadyExistedInTree);
                    }
                }

                // Do person creation
                var newPersonValues = input.SpouseInfo;
                var newSpouse = new Person()
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

                // if there is no previously existing family, we create a new family
                var newRelationship = new Relationship()
                {
                    RelationshipType = RelationshipType.UNKNOWN,
                };
                var newFamily = new Family()
                {
                    Parent1 = operatingPerson.Gender == Gender.MALE ? operatingPerson : newSpouse,
                    Parent2 = operatingPerson.Gender == Gender.FEMALE ? operatingPerson : newSpouse,
                    FamilyTreeId = operatingPerson.FamilyTreeId,
                    Relationship = newRelationship,
                };
                newFamily = await _unitOfWork.Repository<Family>().AddAsync(newFamily);
                await _unitOfWork.SaveChangesAsync();

                // Entry and populate fields
                var entry = _unitOfWork.Entry(newSpouse);
                if (entry != null)
                {
                    await entry.Reference(e => e.FamilyTree).LoadAsync();
                    await entry.Reference(e => e.ChildOfFamily).LoadAsync();
                    await entry.Reference(e => e.ConnectedUser).LoadAsync();

                    if (newSpouse.ChildOfFamily != null)
                    {
                        var entryFamily = _unitOfWork.Entry(newSpouse.ChildOfFamily);
                        await entryFamily.Reference(e => e.Parent1).LoadAsync();
                        await entryFamily.Reference(e => e.Parent2).LoadAsync();
                        await entryFamily.Reference(e => e.Relationship).LoadAsync();
                    }
                }
                await transaction.CommitAsync();
                return new PersonDTO(newSpouse);
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

        public async Task<AddNewChildToFamilyResponseModel> AddNewChild(string userPerformingCreation, AddNewChildToFamilyModel input)
        {
            await using var transaction = await _unitOfWork.CreateTransaction();
            try
            {
                // check if 2 people are valid 
                Person operatingFather = null;
                Person operatingMother = null;
                if (input.FatherId != null) {
                    operatingFather = await _unitOfWork.Repository<Person>().GetDbset().FirstOrDefaultAsync(e => e.Id == input.FatherId);
                    if (operatingFather == null)
                    {
                        throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedPersonFromId);
                    } else if (operatingFather.Gender != Gender.MALE) {
                        throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_FatherGenderIsNotValid);
                    }
                }
                if (input.MotherId != null) {
                    operatingMother = await _unitOfWork.Repository<Person>().GetDbset().FirstOrDefaultAsync(e => e.Id == input.MotherId);
                    if (operatingMother == null)
                    {
                        throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedPersonFromId);
                    }
                    else if (operatingMother.Gender != Gender.FEMALE)
                    {
                        throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_MotherGenderIsNotValid);
                    }
                }
                if (operatingMother == null && operatingFather == null)
                {
                    throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_CannotAddChildToNoFamily);
                }

                // check if user connected to this new parent node is an existing tree node or not (we dont want the user to exist as a tree node)
                if (input.ChildInfo.UserId != null)
                {
                    var connectedUser = await _userManager.FindByIdAsync(input.ChildInfo.UserId);
                    if (connectedUser == null)
                    {
                        throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedUserFromId);
                    }
                    var existingNodeRelatedToUser = await _unitOfWork.Repository<Person>().GetDbset().AnyAsync(e => e.FamilyTreeId == operatingFather.FamilyTreeId && e.FamilyTreeId == operatingMother.FamilyTreeId && e.UserId == connectedUser.Id);
                    if (existingNodeRelatedToUser)
                    {
                        throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_UserAlreadyExistedInTree);
                    }
                }

                // Do person creation
                var newPersonValues = input.ChildInfo;
                var newChild = new Person()
                {
                    FirstName = newPersonValues.FirstName,
                    LastName = newPersonValues.LastName,
                    DateOfBirth = newPersonValues.DateOfBirth,
                    DateOfDeath = newPersonValues.DateOfDeath,
                    Gender = newPersonValues.Gender,
                    Note = newPersonValues.Note,
                    UserId = newPersonValues.UserId,
                    FamilyTreeId = operatingFather != null ? operatingFather.FamilyTreeId : operatingMother.FamilyTreeId,
                };

                // find family
                var response = new AddNewChildToFamilyResponseModel();
                Person parent2 = null;
                if (operatingFather != null && operatingMother != null) {
                    var operatingFamily = await _unitOfWork.Repository<Family>().GetDbset().FirstOrDefaultAsync(e => (e.Parent1Id == operatingFather.Id && e.Parent2Id == operatingMother.Id) || (e.Parent1Id == operatingMother.Id && e.Parent2Id == operatingFather.Id));
                    if(operatingFamily == null)
                    {
                        throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedFamily); ;
                    }
                    newChild.ChildOfFamily = operatingFamily;
                } else {
                    // if there is no previously existing family, we create a new family
                    var newRelationship = new Relationship()
                    {
                        RelationshipType = RelationshipType.UNKNOWN,
                    };
                    parent2 = new Person();
                    parent2.Gender = operatingFather == null ? Gender.MALE : Gender.FEMALE;
                    parent2.FamilyTreeId = operatingFather != null ? operatingFather.FamilyTreeId : operatingMother.FamilyTreeId;
                    
                    var newFamily = new Family()
                    {
                        Parent1 = operatingFather != null ? operatingFather : parent2,
                        Parent2 = operatingMother != null ? operatingMother : parent2,
                        FamilyTreeId = operatingFather == null ? operatingMother.FamilyTreeId : operatingFather.FamilyTreeId,
                        Relationship = newRelationship,
                    };
                    // the new child to the new family
                    newFamily = await _unitOfWork.Repository<Family>().AddAsync(newFamily);
                    newChild.ChildOfFamily = newFamily;
                }
                newChild = await _unitOfWork.Repository<Person>().AddAsync(newChild);
                await _unitOfWork.SaveChangesAsync();

                // Entry and populate fields
                    // entry the child
                var entry = _unitOfWork.Entry(newChild);
                if (entry != null)
                {
                    await entry.Reference(e => e.FamilyTree).LoadAsync();
                    await entry.Reference(e => e.ChildOfFamily).LoadAsync();
                    await entry.Reference(e => e.ConnectedUser).LoadAsync();

                    if (newChild.ChildOfFamily != null)
                    {
                        var entryFamily = _unitOfWork.Entry(newChild.ChildOfFamily);
                        await entryFamily.Reference(e => e.Parent1).LoadAsync();
                        await entryFamily.Reference(e => e.Parent2).LoadAsync();
                        await entryFamily.Reference(e => e.Relationship).LoadAsync();
                    }
                }
                await transaction.CommitAsync();
                    // entry the new parent
                if (parent2 != null)
                {
                    var entryParent = _unitOfWork.Entry(parent2);
                    if (entryParent != null)
                    {
                        await entryParent.Reference(e => e.FamilyTree).LoadAsync();
                        await entryParent.Reference(e => e.ChildOfFamily).LoadAsync();
                        await entry.Reference(e => e.ConnectedUser).LoadAsync();

                        if (newChild.ChildOfFamily != null)
                        {
                            var entryFamily = _unitOfWork.Entry(newChild.ChildOfFamily);
                            await entryFamily.Reference(e => e.Parent1).LoadAsync();
                            await entryFamily.Reference(e => e.Parent2).LoadAsync();
                            await entryFamily.Reference(e => e.Relationship).LoadAsync();
                        }
                    }
                }

                // Populate response
                response.NewChildInfo = new PersonDTO(newChild);
                if(parent2 != null)
                {
                    if(parent2.Gender == Gender.MALE)
                    {
                        response.NewFather = new PersonDTO(parent2);
                    }
                    else
                    {
                        response.NewMother = new PersonDTO(parent2);
                    }
                }
                return response;
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

        public async Task<PersonModel> GetPerson(long id)
        {
            Person person =  await _unitOfWork.Repository<Person>().GetDbset()
                .Include(p => p.ChildOfFamily)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync<Person>();

            if (person == null)
            {
                throw new PersonNotFoundException(PersonServiceExceptionMessages.PersonService_PersonNotFound);
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
                throw new DeletePersonException(PersonServiceExceptionMessages.PersonService_CannotDeletePerson);
            }
            Person deletedPerson = await _unitOfWork.Repository<Person>().DeleteAsync(id);
            return;
        }

        public async Task<PersonModel> UpdatePersonInfo(long personId, PersonInputModel updatedPersonModel)
        {
            Person person = await _unitOfWork.Repository<Person>().FindAsync(personId);

            if (person == null)
            {
                throw new PersonNotFoundException(PersonServiceExceptionMessages.PersonService_PersonNotFound);
            }

            _mapper.Map(updatedPersonModel, person);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PersonModel>(person);
        }
    }
}
