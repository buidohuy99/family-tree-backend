using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.Person;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Infrastructure.Service.InternalServices.CustomException;
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

        public async Task AddNewPerson(AddPersonToTreeModel input)
        {
            return;
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

        public async Task AddChild(AddNewChildToPersonModel input)
        {
            return;
        }

        public async Task AddExistingChild(long personId, long childId)
        {
            return;
        }
    }
}
