using AutoMapper;
using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Helpers.ConfigModels;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.TreeExceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models;
using FamilyTreeBackend.Core.Application.Models.FamilyTree;
using FamilyTreeBackend.Core.Application.Models.FileIO;
using FamilyTreeBackend.Core.Application.Models.Person;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Enums;
using Jose;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StopWord;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    class FamilyTreeService : IFamilyTreeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWEConfig _jweConfig;

        public FamilyTreeService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, IOptions<JWEConfig> jweConfig)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _jweConfig = jweConfig.Value;
        }

        public async Task<FamilyTreeModel> FindFamilyTree(long treeId)
        {
            FamilyTree tree = await _unitOfWork.Repository<FamilyTree>().GetDbset()
                .Include(ft => ft.People)
                .Include(ft => ft.Families)
                .FirstOrDefaultAsync(ft => ft.Id == treeId);

            if (tree == null)
            {
                throw new TreeNotFoundException(TreeExceptionMessages.TreeNotFound, treeId);
            }
            var model = ManualMapTreeToModel(tree);

            return model;
        }

        public async Task<FamilyTreeUpdateResponseModel> UpdateFamilyTree(long treeId, FamilyTreeInputModel model)
        {
            FamilyTree tree = await _unitOfWork.Repository<FamilyTree>().FindAsync(treeId);

            _mapper.Map(model, tree);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FamilyTreeUpdateResponseModel>(tree);

        }


        public async Task DeleteFamilyTree(long treeId)
        {
            var tree = await _unitOfWork.Repository<FamilyTree>().GetDbset()
                .Include(tr => tr.People)
                .Include(tr => tr.Families)
                .SingleOrDefaultAsync(tr => tr.Id == treeId);

            if (tree == null)
            {
                throw new TreeNotFoundException(TreeExceptionMessages.TreeNotFound, treeId);
            }
            
            _unitOfWork.Repository<FamilyTree>().Delete(tree);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<FamilyTreeModel> AddFamilyTree(FamilyTreeInputModel model, ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            if (user == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound);
            }

            var tree = await createDefaultTree(model);
            tree.Owner = user;

            _unitOfWork.Repository<FamilyTree>().Update(tree);
            await _unitOfWork.SaveChangesAsync();

            var responseModel = ManualMapTreeToModel(tree);
            return responseModel;
        }

        public async Task<IEnumerable<FamilyTreeListItemModel>> FindAllTree()
        {
            IEnumerable<FamilyTree> trees = await _unitOfWork.Repository<FamilyTree>().GetDbset()
                .Include(tr => tr.Owner)
                .Include(tr => tr.Editors)
                .Where(tr => tr.PublicMode == true)
                .ToListAsync();

            List<FamilyTreeListItemModel> models = new List<FamilyTreeListItemModel>();
            foreach(var tree in trees)
            {
                models.Add(_mapper.Map<FamilyTreeListItemModel>(tree));
            }

            return models;
        }

        public async Task<IEnumerable<string>> AddUsersToEditor(long treeId, IList<string> userNames)
        {
            var tree = _unitOfWork.Repository<FamilyTree>().GetDbset()
                .Include(tr => tr.Owner)
                .Include(tr => tr.Editors)
                .SingleOrDefault(tr => tr.Id == treeId);

            if (tree == null)
            {
                throw new TreeNotFoundException(TreeExceptionMessages.TreeNotFound, treeId);
            }

            var addedUser = new List<string>();

            foreach(var username in userNames)
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user != null)
                {
                    tree.Editors.Add(user);
                    addedUser.Add(username);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return addedUser;
        }

        public async Task<IEnumerable<FamilyTreeListItemModel>> FindAllTreeAccessibleToUser(ClaimsPrincipal user)
        {
            var applicationUser = await _userManager.GetUserAsync(user);

            if (applicationUser == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound);
            }

            var accessibleTrees = await FindAccessibleTrees(applicationUser);

            List<FamilyTreeListItemModel> models = new List<FamilyTreeListItemModel>();
            foreach (var tree in accessibleTrees)
            {
                models.Add(_mapper.Map<FamilyTreeListItemModel>(tree));
            }

            return models;
        }

        public async Task<IEnumerable<string>> RemoveUsersFromEditor(long treeId, IList<string> userNames)
        {
            var tree = _unitOfWork.Repository<FamilyTree>().GetDbset()
                .Include(tr => tr.Owner)
                .Include(tr => tr.Editors)
                .SingleOrDefault(tr => tr.Id == treeId);

            if (tree == null)
            {
                throw new TreeNotFoundException(TreeExceptionMessages.TreeNotFound, treeId);
            }

            var removedUser = new List<string>();

            foreach (var username in userNames)
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user != null)
                {
                    tree.Editors.Remove(user);
                    removedUser.Add(username);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return removedUser;
        }

        public FamilyTreeContributorsModel GetTreeEditors(long treeId)
        {
            var tree = _unitOfWork.Repository<FamilyTree>().GetDbset()
                .Include(tr => tr.Owner)
                .Include(tr => tr.Editors)
                .SingleOrDefault(tr => tr.Id == treeId);

            if (tree == null)
            {
                throw new TreeNotFoundException(TreeExceptionMessages.TreeNotFound, treeId);
            }

            FamilyTreeContributorsModel contributors = new FamilyTreeContributorsModel()
            {
                Editors = new List<UserDTO>(),
            };
            contributors.Owner = new UserDTO(tree.Owner);

            foreach(var editor in tree.Editors)
            {
                contributors.Editors.Add(new UserDTO(editor));
            }

            return contributors;
        }

        public async Task<IEnumerable<FamilyTreeListItemModel>> FindTreesFromKeywordAccessibleToUser(ClaimsPrincipal user, string keyword)
        {
            var applicationUser = await _userManager.GetUserAsync(user);

            if (applicationUser == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound);
            }

            var query = FindTreesUsingKeyword(keyword);

            if (query == null)
            {
                return new List<FamilyTreeListItemModel>();
            }

            query = query.Include(e => e.Owner).Include(e => e.Editors)
                .Where(e => e.OwnerId == applicationUser.Id || e.Editors.Any(editor => editor.Id == applicationUser.Id));

            List<FamilyTreeListItemModel> trees = new List<FamilyTreeListItemModel>();
            foreach (var tree in (await query.ToListAsync()))
            {
                trees.Add(_mapper.Map<FamilyTreeListItemModel>(tree));
            }

            return trees;
        }

        public async Task<IEnumerable<FamilyTreeListItemModel>> FindTreesFromKeyword(string keyword)
        {
            var query = FindTreesUsingKeyword(keyword);

            if(query == null)
            {
                return new List<FamilyTreeListItemModel>();
            }

            query = query.Include(e => e.Owner).Include(e => e.Editors)
                .Where(tr => tr.PublicMode == true);

            List<FamilyTreeListItemModel> trees = new List<FamilyTreeListItemModel>();
            foreach (var tree in (await query.ToListAsync()))
            {
                trees.Add(_mapper.Map<FamilyTreeListItemModel>(tree));
            }

            return trees;
        }

        public async Task<FamilyTreeModel> ImportFamilyTree(FamilyTreeImportModel model, ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            if (user == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound);
            }

            //Read the file content
            var fileStream = model.ImportedFile.OpenReadStream();
            var reader = new StreamReader(fileStream);
            string token = await reader.ReadToEndAsync();
            fileStream.Close();

            //Decrypt the file content
            FamilyTreeFileIOModel import = null;
            try
            {
                JweToken jwe = JWE.Decrypt(token, _jweConfig.FileIOFamilyTreeKey);
                string content = jwe.Plaintext;
                import = JsonConvert.DeserializeObject<FamilyTreeFileIOModel>(content);
            }
            catch(Exception)
            {
                throw new TreeImportException(TreeExceptionMessages.TreeImportFileDoesNotHaveProperFormat, 0);
            }

            var newFamilyTree = new FamilyTree()
            {
                Name = import.Name,
                Description = import.Description,
                PublicMode = import.PublicMode,
                Owner = user,
            };
            await _unitOfWork.Repository<FamilyTree>().AddAsync(newFamilyTree);

            var eldestPeople = import.People.Where(e => e.ChildOfCoupleId == null).ToList();
            var addedPeople = new Dictionary<long, Person>();
            var addedCouples = new Dictionary<long, Family>();

            while(eldestPeople.Count > 0)
            {
                var person = eldestPeople[0];
                eldestPeople.RemoveAt(0);
                if (!addedPeople.ContainsKey(person.Id))
                {
                    var newPerson = new Person()
                    {
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        DateOfBirth = person.DateOfBirth,
                        DateOfDeath = person.DateOfDeath,
                        Gender = person.Gender,
                        Note = person.Note,
                        ChildOfFamily = person.ChildOfCoupleId != null ? addedCouples[person.ChildOfCoupleId.Value] : null,
                        FamilyTree = newFamilyTree
                    };
                    await _unitOfWork.Repository<Person>().AddAsync(newPerson);
                    addedPeople.Add(person.Id, newPerson);
                }

                foreach(var relationship in person.Spouses)
                {
                    var spouse = import.People.FirstOrDefault(e => e.Id == relationship.SpouseId);
                    if (spouse != null && !addedPeople.ContainsKey(spouse.Id))
                    {
                        var newPerson = new Person()
                        {
                            FirstName = spouse.FirstName,
                            LastName = spouse.LastName,
                            DateOfBirth = spouse.DateOfBirth,
                            DateOfDeath = spouse.DateOfDeath,
                            Gender = spouse.Gender,
                            Note = spouse.Note,
                            ChildOfFamily = spouse.ChildOfCoupleId != null ? addedCouples[spouse.ChildOfCoupleId.Value] : null,
                            FamilyTree = newFamilyTree
                        };
                        await _unitOfWork.Repository<Person>().AddAsync(newPerson);
                        addedPeople.Add(spouse.Id, newPerson);
                    }

                    //Make family with relationship
                    if (!addedCouples.ContainsKey(relationship.CoupleId))
                    {
                        var newFamily = new Family()
                        {
                            Parent1 = addedPeople[person.Id].Gender == Gender.MALE ? addedPeople[person.Id] : (spouse != null ? addedPeople[spouse.Id] : null),
                            Parent2 = addedPeople[person.Id].Gender == Gender.FEMALE ? addedPeople[person.Id] : (spouse != null ? addedPeople[spouse.Id] : null),
                            FamilyTree = newFamilyTree,
                            Relationship = relationship.RelationshipInfo != null ? _mapper.Map<Relationship>(relationship.RelationshipInfo) : null,
                        };
                        await _unitOfWork.Repository<Family>().AddAsync(newFamily);
                        addedCouples.Add(relationship.CoupleId, newFamily);

                        // Only add children into list when create new family, only accept new people into the eldest people queue
                        var children = import.People.Where(e => e.ChildOfCoupleId == relationship.CoupleId && !eldestPeople.Any(el => el.Id == e.Id));
                        eldestPeople.AddRange(children.ToList());
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();
            var mappedTree = ManualMapTreeToModel(newFamilyTree);

            return mappedTree;
        }

        public async Task<(string treeName, string payload)> ExportFamilyTreeJson(long treeId, bool isForBackup)
        {
            var tree = await _unitOfWork.Repository<FamilyTree>().GetDbset()
                .Include(tr => tr.People)
                .Include(tr => tr.Families)
                .ThenInclude(f => f.Relationship)
                .SingleOrDefaultAsync(tr => tr.Id == treeId);

            if (tree == null)
            {
                throw new TreeNotFoundException(TreeExceptionMessages.TreeNotFound, treeId);
            }

            if (isForBackup)
            {
                // Map normal attribs
                var mappedTree = _mapper.Map<FamilyTreeFileIOModel>(tree);
                // Map spouses
                foreach (var personModel in mappedTree.People)
                {
                    Func<Family, bool> whereCondition = null;

                    if (personModel.Gender == Gender.MALE)
                    {
                        whereCondition = (f => f.Parent1Id == personModel.Id);
                    }
                    else
                    {
                        whereCondition = (f => f.Parent2Id == personModel.Id);
                    }

                    var spouses = tree.Families.Where(whereCondition)
                        .Select(e => new FileIOSpouseDTO()
                        {
                            SpouseId = personModel.Gender == Gender.MALE ? e.Parent2Id : e.Parent1Id,
                            CoupleId = e.Id,
                            RelationshipInfo = e.Relationship != null ? _mapper.Map<FileIOSpouseDTO.FileIOSpouseRelationshipDTO>(e.Relationship) : null
                        });

                    personModel.Spouses = spouses;
                }

                string payload = JsonConvert.SerializeObject(mappedTree);

                return (treeName: tree.Name, payload);
            }
            else
            {
                var outputTree = await FindFamilyTree(treeId);

                string payload = JsonConvert.SerializeObject(outputTree);

                return (treeName: tree.Name, payload);
            }
        }

        public async Task<FindTreesPaginationResponseModel> FindAllTree(PaginationModel model)
        {
            var trees = _unitOfWork.Repository<FamilyTree>().GetDbset()
                .Include(tr => tr.Owner)
                .Include(tr => tr.Editors)
                .Where(tr => tr.DateCreated == null || tr.DateCreated.Value.CompareTo(model.CreatedBefore) <= 0)
                .Where(tr => tr.PublicMode == true);

            var totalPage = (ulong)MathF.Ceiling((ulong)trees.Count() / model.ItemsPerPage);
            totalPage = totalPage <= 0 ? 1 : totalPage;

            if (model.Page > totalPage)
            {
                throw new PaginationException(GeneralExceptionMessages.PageOutOfBounds, model.Page, model.ItemsPerPage, totalPage);
            }

            trees = trees.Skip((int)((model.Page - 1) * model.ItemsPerPage)).Take((int)model.ItemsPerPage);

            List<FamilyTreeListItemModel> models = new List<FamilyTreeListItemModel>();
            foreach (var tree in await trees.ToListAsync())
            {
                models.Add(_mapper.Map<FamilyTreeListItemModel>(tree));
            }

            return new FindTreesPaginationResponseModel() { 
                Result = models,
                TotalPages = totalPage,
                CurrentPage = model.Page,
                ItemsPerPage = model.ItemsPerPage
            };
        }

        public async Task<FindTreesPaginationResponseModel> FindAllTreeAccessibleToUser(ClaimsPrincipal user, PaginationModel model)
        {
            var applicationUser = await _userManager.GetUserAsync(user);

            if (applicationUser == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound);
            }

            var trees = _unitOfWork.Repository<FamilyTree>().GetDbset()
                .Include(tr => tr.Owner)
                .Include(tr => tr.Editors)
                .Where(tr => tr.DateCreated == null || tr.DateCreated.Value.CompareTo(model.CreatedBefore) <= 0)
                .Where(tr => tr.OwnerId.Equals(applicationUser.Id) || tr.Editors.Any(e => e.Id.Equals(applicationUser.Id)));

            var totalPage = (ulong)MathF.Ceiling((ulong)trees.Count() / model.ItemsPerPage);
            totalPage = totalPage <= 0 ? 1 : totalPage;

            if (model.Page > totalPage)
            {
                throw new PaginationException(GeneralExceptionMessages.PageOutOfBounds, model.Page, model.ItemsPerPage, totalPage);
            }

            trees = trees.Skip((int)((model.Page - 1) * model.ItemsPerPage)).Take((int)model.ItemsPerPage);

            List<FamilyTreeListItemModel> models = new List<FamilyTreeListItemModel>();
            foreach (var tree in trees)
            {
                models.Add(_mapper.Map<FamilyTreeListItemModel>(tree));
            }

            return new FindTreesPaginationResponseModel()
            {
                Result = models,
                TotalPages = totalPage,
                CurrentPage = model.Page,
                ItemsPerPage = model.ItemsPerPage
            };
        }

        public async Task<FindTreesPaginationResponseModel> FindTreesFromKeyword(string keyword, PaginationModel model)
        {
            var query = FindTreesUsingKeyword(keyword);

            if (query == null)
            {
                return new FindTreesPaginationResponseModel()
                {
                    Result = new List<FamilyTreeListItemModel>(),
                    TotalPages = 1,
                    CurrentPage = 1,
                    ItemsPerPage = model.ItemsPerPage
                };
            }

            query = query.Where(tr => tr.DateCreated == null || tr.DateCreated.Value.CompareTo(model.CreatedBefore) <= 0)
                .Where(tr => tr.PublicMode == true);

            var totalPage = (ulong)MathF.Ceiling((ulong)query.Count() / model.ItemsPerPage);
            totalPage = totalPage <= 0 ? 1 : totalPage;

            if (model.Page > totalPage)
            {
                throw new PaginationException(GeneralExceptionMessages.PageOutOfBounds, model.Page, model.ItemsPerPage, totalPage);
            }

            query = query.Skip((int)((model.Page - 1) * model.ItemsPerPage)).Take((int)model.ItemsPerPage);

            query = query.Include(e => e.Owner).Include(e => e.Editors);

            List<FamilyTreeListItemModel> trees = new List<FamilyTreeListItemModel>();
            foreach (var tree in (await query.ToListAsync()))
            {
                trees.Add(_mapper.Map<FamilyTreeListItemModel>(tree));
            }

            return new FindTreesPaginationResponseModel()
            {
                Result = trees,
                TotalPages = totalPage,
                CurrentPage = model.Page,
                ItemsPerPage = model.ItemsPerPage
            };
        }

        public async Task<FindTreesPaginationResponseModel> FindTreesFromKeywordAccessibleToUser(ClaimsPrincipal user, string keyword, PaginationModel model)
        {
            var applicationUser = await _userManager.GetUserAsync(user);

            if (applicationUser == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound);
            }

            var query = FindTreesUsingKeyword(keyword);

            if (query == null)
            {
                return new FindTreesPaginationResponseModel()
                {
                    Result = new List<FamilyTreeListItemModel>(),
                    TotalPages = 1,
                    CurrentPage = 1,
                    ItemsPerPage = model.ItemsPerPage
                };
            }

            query = query.Where(tr => tr.DateCreated == null || tr.DateCreated.Value.CompareTo(model.CreatedBefore) <= 0)
                .Where(tr => tr.OwnerId.Equals(applicationUser.Id) || tr.Editors.Any(e => e.Id.Equals(applicationUser.Id)));

            var totalPage = (ulong)MathF.Ceiling((ulong)query.Count() / model.ItemsPerPage);
            totalPage = totalPage <= 0 ? 1 : totalPage;

            if (model.Page > totalPage)
            {
                throw new PaginationException(GeneralExceptionMessages.PageOutOfBounds, model.Page, model.ItemsPerPage, totalPage);
            }

            query = query.Skip((int)((model.Page - 1) * model.ItemsPerPage)).Take((int)model.ItemsPerPage);

            query = query.Include(e => e.Owner).Include(e => e.Editors);

            List<FamilyTreeListItemModel> trees = new List<FamilyTreeListItemModel>();
            foreach (var tree in (await query.ToListAsync()))
            {
                trees.Add(_mapper.Map<FamilyTreeListItemModel>(tree));
            }

            return new FindTreesPaginationResponseModel()
            {
                Result = trees,
                TotalPages = totalPage,
                CurrentPage = model.Page,
                ItemsPerPage = model.ItemsPerPage
            };
        }

        #region Helper methods

        private async Task<IEnumerable<FamilyTree>> FindAccessibleTrees(ApplicationUser applicationUser)
        {
            var param = new SqlParameter("User", applicationUser.Id);
            var query = _unitOfWork.Repository<FamilyTree>().GetDbset()
                .FromSqlRaw(Sql_FindUserAllTrees, param)
                .Include(tr => tr.Owner)
                .Include(tr => tr.Editors);

            //var hello =  query.ToQueryString();

            return await query
                
                .ToListAsync();
        }

        private IQueryable<FamilyTree> FindTreesUsingKeyword(string keyword)
        {
            var keywordWithoutStopwords = keyword.RemoveStopWords("en").RemoveStopWords("vi").ToLower();

            MatchCollection matches = Regex.Matches(keywordWithoutStopwords, "[a-z]([:'-]?[a-z])*",
                                        RegexOptions.IgnoreCase);

            var treeQuery = _unitOfWork.Repository<FamilyTree>().GetDbset()
                    .AsQueryable();

            bool atLeastOneMatchFound = false;
            foreach (Match match in matches)
            {
                if (!atLeastOneMatchFound && treeQuery.Any(e => e.Name.ToLower().Contains(match.Value) || e.Description.ToLower().Contains(match.Value)))
                {
                    atLeastOneMatchFound = true;
                }
                treeQuery = treeQuery.Where(e => e.Name.ToLower().Contains(match.Value) || e.Description.ToLower().Contains(match.Value));
            }

            if (!atLeastOneMatchFound && keyword != string.Empty)
            {
                return null;
            }

            return treeQuery;
        }

        private async Task<FamilyTree> createDefaultTree(FamilyTreeInputModel model)
        {
            FamilyTree familyTree = _mapper.Map<FamilyTree>(model);

            familyTree.People = new List<Person>();
            familyTree.Families = new List<Family>();

            await _unitOfWork.Repository<FamilyTree>().AddAsync(familyTree);

            Person person = new Person
            {
                FirstName = "Person",
                LastName = "Unknown",
                Gender = Gender.MALE,
            };

            Person father = new Person
            {
                FirstName = "Father",
                LastName = "Unknown",
                Gender = Gender.MALE,
            };

            Person mother = new Person
            {
                FirstName = "Mother",
                LastName = "Unknown",
                Gender = Gender.FEMALE,
            };

            familyTree.People.Add(person);
            familyTree.People.Add(mother);
            familyTree.People.Add(father);

            Family family = new Family
            {
                Parent1 = father,
                Parent2 = mother,
                Children = new List<Person>(),
            };

            family.Children.Add(person);

            Relationship relationship = new Relationship
            {
                RelationshipType = RelationshipType.MARRIED,
            };

            family.Relationship = relationship;

            familyTree.Families.Add(family);

            await _unitOfWork.SaveChangesAsync();

            return familyTree;
        }

        private FamilyTreeModel ManualMapTreeToModel(FamilyTree tree)
        {
            var model = _mapper.Map<FamilyTreeModel>(tree);

            AddPersonSpousesToModel(tree, model);

            return model;
        }

        private void AddPersonSpousesToModel(FamilyTree tree, FamilyTreeModel model)
        {
            foreach (var personModel in model.People)
            {
                var spouses = FindPersonSpousesFromTree(personModel.Id, personModel.Gender, tree);
                IEnumerable<PersonModel> models = _mapper.Map<IEnumerable<Person>, IEnumerable<PersonModel>>(spouses);
                personModel.Spouses = models;
            }
        }

        private IEnumerable<Person> FindPersonSpousesFromTree(long personId, Gender gender, FamilyTree tree)
        {
            Func<Family, bool> whereCondition = null;
            Func<Family, Person> selectOption = null;

            if (gender == Gender.MALE)
            {
                whereCondition = (f => f.Parent1Id == personId && f.Parent2Id != null);
                selectOption = (f => f.Parent2);
            }
            else
            {
                whereCondition = (f => f.Parent2Id == personId && f.Parent1Id != null);
                selectOption = (f => f.Parent1);
            }

            IEnumerable<Person> people = tree.Families
                .Where(whereCondition)
                .Select(selectOption)
                .ToList();

            return people;
        }
        #endregion

        private const string Sql_FindUserAllTrees = @"
        SELECT distinct tree.*
        FROM FamilyTree tree 
            LEFT JOIN ApplicationUserFamilyTree editors ON tree.Id = editors.EditorOfFamilyTreesId
            LEFT JOIN Person persons ON  tree.Id = persons.FamilyTreeId
        WHERE tree.OwnerId = @User 
		OR editors.EditorsId = @User 
		OR persons.UserId = @User";
    }
}
