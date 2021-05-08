using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Models.Person;
﻿using FamilyTreeBackend.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IPersonService
    {
        public Task<AddNewParentToPersonResponseModel> AddNewParent(AddNewParentToPersonModel input);
        public Task<PersonDTO> AddNewSpouse(AddNewSpouseToPersonModel input);
        public Task<AddNewChildToFamilyResponseModel> AddNewChild(AddNewChildToFamilyModel input);
        public Task<PersonModel> GetPerson(long id);
        public Task<IEnumerable<PersonModel>> GetPersonChildren(long id);
        public Task RemovePerson(long id);
        public Task<PersonModel> UpdatePersonInfo(long personId, PersonInputModel updatedPersonModel);
    }
}
