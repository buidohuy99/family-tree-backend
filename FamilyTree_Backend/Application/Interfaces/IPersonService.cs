using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Models.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IPersonService
    {
        public Task<PersonDTO> AddNewPerson(string userPerformingCreation, AddPersonToTreeModel input);

        public Task AddNewParent(AddNewParentToPersonModel input);

        public Task AddExistingParent(long personId, long parentId);

        public Task AddNewSpouse(AddNewSpouseToPersonModel input);

        public Task AddExistingSpouse(long personId, long spouseId);

        public Task AddNewChild(AddNewChildToPersonModel input);

        public Task AddExistingChild(long personId, long childId);
    }
}
