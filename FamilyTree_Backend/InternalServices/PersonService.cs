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

                // Lastly, check if the operating person is in any family
                var connectedFamily = await _unitOfWork.Repository<Family>().GetDbset().FirstOrDefaultAsync(e => e.Id == operatingPerson.ChildOf);
                if(connectedFamily != null)
                {
                    // check for an empty slot to insert in a parent if a family is found
                    if (connectedFamily.Parent1Id == null)
                    {
                        connectedFamily.Parent1 = newParent;
                    }
                    else if (connectedFamily.Parent2Id == null)
                    {
                        connectedFamily.Parent2 = newParent;
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
                        Parent1 = newParent,
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

        public async Task<FamilyDTO> AddNewSpouse(string userPerformingCreation, AddNewSpouseToPersonModel input)
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
                    Parent1Id = operatingPerson.Id,
                    Parent2 = newSpouse,
                    FamilyTreeId = operatingPerson.FamilyTreeId,
                    Relationship = newRelationship,
                };
                newFamily = await _unitOfWork.Repository<Family>().AddAsync(newFamily);
                await _unitOfWork.SaveChangesAsync();

                // Entry and populate fields
                var entry = _unitOfWork.Entry(newFamily);
                if (entry != null)
                {
                    await entry.Reference(e => e.Parent1).LoadAsync();
                    await entry.Reference(e => e.Parent2).LoadAsync();
                    await entry.Reference(e => e.Relationship).LoadAsync();
                }
                await transaction.CommitAsync();
                return new FamilyDTO(newFamily);
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

        public async Task<PersonDTO> AddNewChild(string userPerformingCreation, AddNewChildToPersonModel input)
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
                if (input.ChildInfo.UserId != null)
                {
                    var connectedUser = await _userManager.FindByIdAsync(input.ChildInfo.UserId);
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
                    FamilyTreeId = operatingPerson.FamilyTreeId
                };

                // Lastly, check if the operating person is in any family
                var connectedFamilies = _unitOfWork.Repository<Family>().GetDbset().Where(e => e.Parent1Id == operatingPerson.Id || e.Parent2Id == operatingPerson.Id);
                if (connectedFamilies.Any())
                {
                    // More than one family, you need to use the family-management route
                    if(connectedFamilies.Count() > 1)
                    {
                        throw new PersonServiceException(PersonServiceExceptionMessages.PersonService_MultipleFamiliesFoundOfPerson_DontKnowWhichToAddChild);
                    }
                    //else we add child to the single family found
                    var connectedFamily = connectedFamilies.ToList()[0];
                    newChild.ChildOfFamily = connectedFamily;
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
                        Parent1Id = operatingPerson.Id,
                        FamilyTreeId = operatingPerson.FamilyTreeId,
                        Relationship = newRelationship,
                    };
                    // the new child to the new family
                    newFamily = await _unitOfWork.Repository<Family>().AddAsync(newFamily);
                    newChild.ChildOfFamily = newFamily;
                }
                newChild = await _unitOfWork.Repository<Person>().AddAsync(newChild);
                await _unitOfWork.SaveChangesAsync();

                // Entry and populate fields
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
                return new PersonDTO(newChild);
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
    }
}
