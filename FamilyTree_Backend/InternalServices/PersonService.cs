using AutoMapper;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.PersonModels;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Infrastructure.Service.InternalServices.CustomException;
using Microsoft.EntityFrameworkCore;
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

        private readonly IMapper _mapper;

        public PersonService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
