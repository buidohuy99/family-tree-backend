using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.Person;
using FamilyTreeBackend.Core.Domain.Constants;
﻿using AutoMapper;
using FamilyTreeBackend.Core.Application.Models;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

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

        public async Task<AddNewParentToPersonResponseModel> AddNewParent(AddNewParentToPersonModel input)
        {
            await using var transaction = await _unitOfWork.CreateTransaction();
            try
            {
                // check person is valid 
                var operatingPerson = await _unitOfWork.Repository<Person>().GetDbset().FirstOrDefaultAsync(e => e.Id == input.PersonId);
                if (operatingPerson == null)
                {
                    throw new PersonNotFoundException(PersonExceptionMessages.PersonNotFound, input.PersonId);
                }

                // check if user connected to this new parent node is an existing tree node or not (we dont want the user to exist as a tree node)
                if (input.ParentInfo.UserId != null)
                {
                    var connectedUser = await _userManager.FindByIdAsync(input.ParentInfo.UserId);
                    if (connectedUser == null)
                    {
                        throw new UserNotFoundException(UserExceptionMessages.UserNotFound, input.ParentInfo.UserId);
                    }
                    var existingNodeRelatedToUser = await _unitOfWork.Repository<Person>().GetDbset().AnyAsync(e => e.FamilyTreeId == operatingPerson.FamilyTreeId && e.UserId == input.ParentInfo.UserId);
                    if (existingNodeRelatedToUser)
                    {

                        throw new UserExistsInTreeException(PersonExceptionMessages.UserAlreadyExistedInTree, input.ParentInfo.UserId, operatingPerson.FamilyTreeId);
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
                    FamilyTreeId = operatingPerson.FamilyTreeId,
                    ImageUrl = newPersonValues.ImageUrl
                };

                // Lastly, check if the operating person is in any family
                var connectedFamily = _unitOfWork.Repository<Family>().GetDbset().FirstOrDefault(e => e.Id == operatingPerson.ChildOf);
                Family newFamily = null;
                if(connectedFamily != null)
                {
                    newFamily = connectedFamily;
                    // Family is full
                    if (newFamily.Parent1Id != null && newFamily.Parent2Id != null)
                    {
                        throw new CannotAddParentException(PersonExceptionMessages.FamilyAlreadyFull, operatingPerson.Id);
                    }
                    // Family is empty
                    if (newFamily.Parent1Id == null && newFamily.Parent2Id == null)
                    {
                        switch (newPersonValues.Gender)
                        {
                            case Gender.MALE:
                                newFamily.Parent1 = newParent;
                                break;
                            case Gender.FEMALE:
                                newFamily.Parent2 = newParent;
                                break;
                        }
                    }
                    else // Family have one empty slot
                    {
                        if (newFamily.Parent1Id == null) // lacks a father
                        {
                            if (newPersonValues.Gender == Gender.FEMALE)
                            {
                                throw new InvalidGenderException(PersonExceptionMessages.FatherGenderIsNotValid, null, newPersonValues.Gender);
                            }
                            newFamily.Parent1 = newParent;
                        }
                        else if (newFamily.Parent2Id == null)
                        {
                            if (newPersonValues.Gender == Gender.MALE)
                            {
                                throw new InvalidGenderException(PersonExceptionMessages.MotherGenderIsNotValid, null, newPersonValues.Gender);
                            }
                            newFamily.Parent2 = newParent;
                        }
                    }
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
                throw;
            }
        }

        public async Task<PersonDTO> AddNewSpouse(AddNewSpouseToPersonModel input)
        {
            await using var transaction = await _unitOfWork.CreateTransaction();
            try
            {
                // check person is valid 
                var operatingPerson = await _unitOfWork.Repository<Person>().GetDbset().FirstOrDefaultAsync(e => e.Id == input.PersonId);
                if (operatingPerson == null)
                {
                    throw new PersonNotFoundException(PersonExceptionMessages.PersonNotFound, input.PersonId);
                }

                // Check gender validity
                if (operatingPerson.Gender == input.SpouseInfo.Gender)
                {
                    throw new InvalidGenderException(PersonExceptionMessages.SpouseGenderNotValid, operatingPerson.Id, operatingPerson.Gender);
                }

                // check if user connected to this new parent node is an existing tree node or not (we dont want the user to exist as a tree node)
                if (input.SpouseInfo.UserId != null)
                {
                    var connectedUser = await _userManager.FindByIdAsync(input.SpouseInfo.UserId);
                    if (connectedUser == null)
                    {
                        throw new UserNotFoundException(UserExceptionMessages.UserNotFound, input.SpouseInfo.UserId);
                    }
                    var existingNodeRelatedToUser = await _unitOfWork.Repository<Person>().GetDbset().AnyAsync(e => e.FamilyTreeId == operatingPerson.FamilyTreeId && e.UserId == connectedUser.Id);
                    if (existingNodeRelatedToUser)
                    {
                        throw new UserExistsInTreeException(PersonExceptionMessages.UserAlreadyExistedInTree, connectedUser.Id, operatingPerson.FamilyTreeId);
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
                    FamilyTreeId = operatingPerson.FamilyTreeId,
                    ImageUrl = newPersonValues.ImageUrl
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
                throw;
            }
        }

        public async Task<AddNewChildToFamilyResponseModel> AddNewChild(AddNewChildToFamilyModel input)
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
                        throw new PersonNotFoundException(
                            PersonExceptionMessages.PersonNotFound,
                            input.FatherId.Value);
                    } else if (operatingFather.Gender != Gender.MALE) {
                        throw new InvalidGenderException(
                            PersonExceptionMessages.FatherGenderIsNotValid, 
                            operatingFather.Id, 
                            operatingFather.Gender);
                    }
                }
                if (input.MotherId != null) {
                    operatingMother = await _unitOfWork.Repository<Person>().GetDbset().FirstOrDefaultAsync(e => e.Id == input.MotherId);
                    if (operatingMother == null)
                    {
                        throw new PersonNotFoundException(
                            PersonExceptionMessages.PersonNotFound,
                            input.MotherId.Value);
                    }
                    else if (operatingMother.Gender != Gender.FEMALE)
                    {
                        throw new InvalidGenderException(
                            PersonExceptionMessages.MotherGenderIsNotValid, 
                            operatingMother.Id, 
                            operatingMother.Gender);
                    }
                }

                // invalid family to insert
                if(operatingFather == null && operatingMother == null)
                {
                    throw new FamilyNotFoundException(PersonExceptionMessages.FamilyNotFound);
                }
                
                // check if user connected to this new parent node is an existing tree node or not (we dont want the user to exist as a tree node)
                if (input.ChildInfo.UserId != null)
                {
                    var connectedUser = await _userManager.FindByIdAsync(input.ChildInfo.UserId);
                    if (connectedUser == null)
                    {
                        throw new UserNotFoundException(UserExceptionMessages.UserNotFound, input.ChildInfo.UserId);
                    }
                    var existingNodeRelatedToUser = await _unitOfWork.Repository<Person>().GetDbset().AnyAsync(e => e.FamilyTreeId == operatingFather.FamilyTreeId && e.FamilyTreeId == operatingMother.FamilyTreeId && e.UserId == connectedUser.Id);
                    if (existingNodeRelatedToUser)
                    {
                        throw new UserExistsInTreeException(PersonExceptionMessages.UserAlreadyExistedInTree, connectedUser.Id, operatingFather.FamilyTreeId);
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
                    ImageUrl = newPersonValues.ImageUrl,
                };

                // find family
                var response = new AddNewChildToFamilyResponseModel();
                Person parent2 = null;
                if (operatingFather != null && operatingMother != null) {
                    var operatingFamily = await _unitOfWork.Repository<Family>().GetDbset().FirstOrDefaultAsync(e => (e.Parent1Id == operatingFather.Id && e.Parent2Id == operatingMother.Id) || (e.Parent1Id == operatingMother.Id && e.Parent2Id == operatingFather.Id));
                    if(operatingFamily == null)
                    {
                        throw new FamilyNotFoundException(PersonExceptionMessages.FamilyNotFound);
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
                throw;
            }
        }

        public async Task<PersonModel> GetPerson(long id)
        {
            Person person =  await _unitOfWork.Repository<Person>().GetDbset()
                .Include(p => p.ChildOfFamily)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (person == null)
            {
                throw new PersonNotFoundException(PersonExceptionMessages.PersonNotFound, id);
            }

            var personModel = _mapper.Map<Person, PersonModel>(person);

            var spouses = await FindPersonSpouses(person);

            IEnumerable<PersonModel> models = _mapper.Map<IEnumerable<Person>, IEnumerable<PersonModel>>(spouses);

            personModel.Spouses = models;
            return personModel;
        }

        public async Task<IEnumerable<Person>> FindPersonSpouses(Person person)
        {
            //IQueryable<Family> query = _unitOfWork.Repository<Family>().GetDbset();

            if (person.Gender == Gender.MALE)
            {
                IEnumerable<Person> people = await _unitOfWork.Repository<Family>().GetDbset()
                .Include(f => f.Parent2)
                .Where(f => f.Parent1Id == person.Id)
                .OrderBy(f => f.Relationship.StartDate)
                .Select(f => f.Parent2)
                .ToListAsync();

                string query = _unitOfWork.Repository<Family>().GetDbset()
                .Include(f => f.Parent2)
                .Where(f => f.Parent1Id == person.Id)
                .OrderBy(f => f.Relationship.StartDate)
                .Select(f => f.Parent2).ToQueryString();

                return people;
            }
            else
            {
                IEnumerable<Person> people = await _unitOfWork.Repository<Family>().GetDbset()
                .Include(f => f.Parent1)
                .Where(f => f.Parent2Id == person.Id)
                .Select(f => f.Parent1)
                .ToListAsync();

                return people;
            }

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
            var operatingPerson = _unitOfWork.Repository<Person>().GetDbset().Where(e => e.Id == id)
                                .Include(e => e.ChildOfFamily)
                                .Include(e => e.ChildOfFamily.Children)
                                .FirstOrDefault();

            if(operatingPerson == null)
            {
                throw new PersonNotFoundException(PersonExceptionMessages.PersonNotFound, id);
            }

            var parents = operatingPerson.ChildOfFamily;

            var families = _unitOfWork.Repository<Family>().GetDbset().Where(e => e.Parent1Id == id || e.Parent2Id == id);

            if(parents != null)
            {
                if(families.Count() > 0)
                {
                    throw new DeletePersonException(PersonExceptionMessages.TreeDivergenceAfterDeletion, id);
                }
                _unitOfWork.Repository<Person>().GetDbset().Attach(operatingPerson).State = EntityState.Deleted;
                //Check if he is child of any single parent family with him as the only child left, if he is remove the family from db
                if ((parents.Parent1Id == null || parents.Parent2Id == null) && parents.Children.Count <= 1)
                {
                    _unitOfWork.Repository<Family>().GetDbset().Attach(parents).State = EntityState.Deleted;
                }
            }
            else
            {
                if(families.Count() <= 0)
                {
                    throw new DeletePersonException(PersonExceptionMessages.CannotDeleteOnlyPersonInTree, id);
                }
                else if(families.Count() == 1)
                {
                    var operatingFamily = families.Include(e => e.Children).FirstOrDefault();
                    // if he she is a single parent of the family
                    if((operatingFamily.Parent1Id == id && operatingFamily.Parent2Id == null) || (operatingFamily.Parent2Id == id && operatingFamily.Parent1Id == null))
                    {
                        if(operatingFamily.Children.Count <= 0) // single parent of a family with no children - this is to catch DB inconsistency
                        {
                            _unitOfWork.Repository<Family>().GetDbset().Attach(operatingFamily).State = EntityState.Deleted;
                            await _unitOfWork.SaveChangesAsync();
                            throw new DeletePersonException(PersonExceptionMessages.CannotDeleteOnlyPersonInTree, id);
                        } else if (operatingFamily.Children.Count > 1){ 
                            throw new DeletePersonException(PersonExceptionMessages.TreeDivergenceAfterDeletion, id);
                        }

                        var onlyChild = operatingFamily.Children.ToList()[0];
                        onlyChild.ChildOfFamily = null;
                        _unitOfWork.Repository<Person>().Update(onlyChild);

                        _unitOfWork.Repository<Family>().GetDbset().Attach(operatingFamily).State = EntityState.Deleted;
                        _unitOfWork.Repository<Person>().GetDbset().Attach(operatingPerson).State = EntityState.Deleted;

                    }
                    else // not a single parent
                    {
                        if(operatingFamily.Parent1Id == id)
                        {
                            operatingFamily.Parent1 = null;
                        } else if (operatingFamily.Parent2Id == id)
                        {
                            operatingFamily.Parent2 = null;
                        }
                        _unitOfWork.Repository<Family>().Update(operatingFamily);
                        _unitOfWork.Repository<Person>().GetDbset().Attach(operatingPerson).State = EntityState.Deleted;
                        if(operatingFamily.Children.Count <= 0) // Family become
                        {
                            _unitOfWork.Repository<Family>().GetDbset().Attach(operatingFamily).State = EntityState.Deleted;
                        }
                    }
                }
                else
                {
                    throw new DeletePersonException(PersonExceptionMessages.TreeDivergenceAfterDeletion, id);
                }
            }
            await _unitOfWork.SaveChangesAsync();

            return;
        }

        public async Task<PersonModel> UpdatePersonInfo(long personId, PersonInputModel updatedPersonModel)
        {
            Person person = await _unitOfWork.Repository<Person>().FindAsync(personId);

            if (person == null)
            {
                throw new PersonNotFoundException(PersonExceptionMessages.PersonNotFound, personId);
            }

            if(updatedPersonModel.UserId != null && _userManager.FindByIdAsync(updatedPersonModel.UserId) == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound, updatedPersonModel.UserId);
            }

            _mapper.Map(updatedPersonModel, person);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PersonModel>(person);
        }

        public async Task<PersonDetailsModel> GetPersonDetail(long personId)
        {
            var person = await _unitOfWork.Repository<Person>().GetDbset()
                .Include(p => p.ChildOfFamily)
                .ThenInclude(f => f.Parent1)
                .Include(p => p.ChildOfFamily)
                .ThenInclude(f => f.Parent2)
                .Where(p => p.Id == personId)
                .SingleOrDefaultAsync();
            if (person == null)
            {
                throw new PersonNotFoundException(PersonExceptionMessages.PersonNotFound, personId);
            }

            //Get family details, including spouses and children
            var queryForFamily = _unitOfWork.Repository<Family>().GetDbset();

            Expression<Func<Family, bool>> whereCondition = null;
            Expression<Func<Family, Person>> includeOption = null;
            if (person.Gender == Gender.MALE)
            {
                whereCondition = (f => f.Parent1Id == personId);
                includeOption = (f => f.Parent2);
            }
            else
            {
                whereCondition = (f => f.Parent2Id == personId);
                includeOption = (f => f.Parent1);
            }

            var families = queryForFamily
                .Include(includeOption)
                .Include(f => f.Relationship)
                .Include(f => f.Children)
                .Where(whereCondition)
                .OrderBy(f => f.Relationship.StartDate).ToList();
                

            List<SpouseDetailDTO> spouseDetails = new List<SpouseDetailDTO>();
            List<PersonSummaryDTO> childrenSummary = new List<PersonSummaryDTO>();

            foreach (var family in families)
            {
                var spouseSummary = person.Gender == Gender.MALE?
                    _mapper.Map<PersonSummaryDTO>(family.Parent2)
                    : _mapper.Map<PersonSummaryDTO>(family.Parent1);
                var relationshipDto = _mapper.Map<RelationshipDTO>(family.Relationship);
                spouseDetails.Add(new SpouseDetailDTO(spouseSummary, relationshipDto));

                var children = _mapper.Map<IEnumerable<Person>, List<PersonSummaryDTO>>(family.Children);
                childrenSummary.AddRange(children);
            }

            var personDetail = _mapper.Map<PersonDetailsModel>(person);
            personDetail.Spouses = spouseDetails;
            personDetail.Children = childrenSummary;

            return personDetail;
        }

        public async Task<PersonDetailsResponseModel> UpdatePersonDetails(long personId, PersonDetailsUpdateModel input)
        {
            var person = await _unitOfWork.Repository<Person>().FindAsync(personId);
            if (person == null)
            {
                throw new PersonNotFoundException(PersonExceptionMessages.PersonNotFound, personId);
            }

            //check if user already in the tree
            var existingUserId = await _unitOfWork.Repository<Person>().GetDbset()
                .AnyAsync(p => p.Id.Equals(input.UserId) && p.Id != personId);
            if (existingUserId)
            {
                throw new UserExistsInTreeException(
                    PersonExceptionMessages.UserAlreadyExistedInTree,
                    input.UserId, person.FamilyTreeId);
            }
            
            _mapper.Map(input, person);

            List<Family> updatedFamilies = new List<Family>();

            Expression<Func<Family, bool>> whereCondition = null;
            if (person.Gender == Gender.MALE)
            {
                whereCondition = (f => f.Parent1Id == personId);
            }
            else
            {
                whereCondition = (f => f.Parent2Id == personId);
            }
            var families = await _unitOfWork.Repository<Family>().GetDbset()
                    .Include(f => f.Relationship)
                    .Where(whereCondition)
                    .ToListAsync();

            foreach (var model in input.SpouseRelationships)
            {
               Func<Family, bool> spouseCompareOption = null;
                if (person.Gender == Gender.MALE)
                {
                    spouseCompareOption = (f => f.Parent2Id == model.SpouseId);
                }
                else
                {
                    spouseCompareOption = (f => f.Parent1Id == model.SpouseId);
                }

                var family = families.Where(spouseCompareOption).SingleOrDefault();
                if (family != null)
                {
                    _mapper.Map(model, family.Relationship);
                    updatedFamilies.Add(family);
                }
            }
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<PersonDetailsResponseModel>(person);

            List<SpouseRelationshipUpdateModel> relationships = new List<SpouseRelationshipUpdateModel>();
            foreach(var family in updatedFamilies)
            {
                var relationship = _mapper.Map<SpouseRelationshipUpdateModel>(family.Relationship);
                relationship.SpouseId = person.Gender == Gender.MALE ?
                    family.Parent2Id.Value : family.Parent1Id.Value;
                relationships.Add(relationship);
            }
            result.SpouseRelationships = relationships;
            return result;
        }
    }
}
