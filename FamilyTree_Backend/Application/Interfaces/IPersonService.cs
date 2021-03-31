using FamilyTreeBackend.Core.Application.Models.PersonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IPersonService
    {
        public Task<PersonModel> GetPerson(long id);
        public Task<IEnumerable<PersonModel>> GetPersonChildren(long id);
        public Task RemovePerson(long id);
    }
}
