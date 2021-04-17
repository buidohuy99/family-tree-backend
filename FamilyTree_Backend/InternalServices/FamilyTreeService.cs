using AutoMapper;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.FamilyTreeService;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models;
using FamilyTreeBackend.Core.Application.Models.FamilyTree;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    class FamilyTreeService : IFamilyTreeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FamilyTreeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FamilyTreeModel> FindFamilyTree(long treeId)
        {
            FamilyTree tree = await _unitOfWork.Repository<FamilyTree>().GetDbset()
                .Include(ft => ft.People)
                .FirstOrDefaultAsync(ft => ft.Id == treeId);

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
            var tree = await _unitOfWork.Repository<FamilyTree>().FindAsync(treeId);

            if (tree == null)
            {
                throw new TreeNotFoundException(treeId);
            }

            _unitOfWork.Repository<FamilyTree>().Delete(tree);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<FamilyTreeModel> AddFamilyTree(FamilyTreeInputModel model)
        {
            var tree = await createDefaultTree(model);
            var responseModel = ManualMapTreeToModel(tree);

            return responseModel;

        }

        public async Task<IEnumerable<FamilyTreeListItemModel>> FindAllTree()
        {
            IEnumerable<FamilyTree> trees = await _unitOfWork.Repository<FamilyTree>().GetDbset()
                .ToListAsync();

            List<FamilyTreeListItemModel> models = new List<FamilyTreeListItemModel>();
            foreach(var tree in trees)
            {
                models.Add(_mapper.Map<FamilyTreeListItemModel>(tree));
            }

            return models;
        }

        private async Task<FamilyTree> createDefaultTree(FamilyTreeInputModel model)
        {
            FamilyTree familyTree = _mapper.Map<FamilyTree>(model);

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

            Marriage relationship = new Marriage
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
                whereCondition = (f => f.Parent1Id == personId);
                selectOption = (f => f.Parent2);
            }
            else
            {
                whereCondition = (f => f.Parent2Id == personId);
                selectOption = (f => f.Parent1);
            }

            IEnumerable<Person> people = tree.Families
                .Where(whereCondition)
                .Select(selectOption)
                .ToList();

            return people;
        }
    }
}
