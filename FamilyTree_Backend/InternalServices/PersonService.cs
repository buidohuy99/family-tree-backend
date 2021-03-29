using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.PersonModels;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Infrastructure.Service.InternalServices.AutoMapper;
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

        public PersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PersonModel> GetPerson(long id)
        {
            Person person =  await _unitOfWork.Repository<Person>().GetDbset()
                .Include(p => p.ChildOfFamily)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync<Person>();

            var personModel = Mapping.Mapper.Map<Person, PersonModel>(person);

            return personModel;
        }

        public Task<IEnumerable<PersonModel>> GetPersonChildren(long id)
        {
            throw new NotImplementedException();
        }
    }
}
