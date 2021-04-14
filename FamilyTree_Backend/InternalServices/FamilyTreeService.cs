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

            var model = _mapper.Map<FamilyTreeModel>(tree);

            foreach(var personModel in model.People)
            {
                var spouses = await FindPersonSpouses(personModel.Id, personModel.Gender);
                IEnumerable<PersonModel> models = _mapper.Map<IEnumerable<Person>, IEnumerable<PersonModel>>(spouses);
                personModel.Spouses = models;
            }

            return model;
        }

        private async Task<IEnumerable<Person>> FindPersonSpouses(long personId, Gender gender)
        {
            //IQueryable<Family> query = _unitOfWork.Repository<Family>().GetDbset();

            if (gender == Gender.MALE)
            {
                IEnumerable<Person> people = await _unitOfWork.Repository<Family>().GetDbset()
                .Include(f => f.Parent2)
                .Where(f => f.Parent1Id == personId)
                .Select(f => f.Parent2)
                .ToListAsync();

                string query = _unitOfWork.Repository<Family>().GetDbset()
                .Include(f => f.Parent2)
                .Where(f => f.Parent1Id == personId)
                .Select(f => f.Parent2).ToQueryString();

                return people;
            }
            else
            {
                IEnumerable<Person> people = await _unitOfWork.Repository<Family>().GetDbset()
                .Include(f => f.Parent1)
                .Where(f => f.Parent2Id == personId)
                .Select(f => f.Parent1)
                .ToListAsync();

                return people;
            }

        }

        public async Task<FamilyTreeUpdateResponseModel> UpdateFamilyTree(long treeId, FamilyTreeInputModel model)
        {
            FamilyTree tree = await _unitOfWork.Repository<FamilyTree>().FindAsync(treeId);

            _mapper.Map(model, tree);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FamilyTreeUpdateResponseModel>(tree);

        }
    }
}
