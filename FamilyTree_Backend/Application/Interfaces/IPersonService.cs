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

        public Task<PersonDTO> AddNewParent(string userPerformingCreation, AddNewParentToPersonModel input);

        public Task<PersonDTO> AddExistingParent(string userPerformingCreation, long personId, long parentId);

        public Task<PersonDTO> AddNewSpouse(string userPerformingCreation, AddNewSpouseToPersonModel input);

        public Task<PersonDTO> AddExistingSpouse(string userPerformingCreation, long personId, long spouseId);

        public Task<PersonDTO> AddNewChild(string userPerformingCreation, AddNewChildToPersonModel input);

        public Task<PersonDTO> AddExistingChild(string userPerformingCreation, long personId, long childId);
    }
}
