using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions
{
    [Serializable]
    public class PersonNotFoundException : PersonException
    {
        public PersonNotFoundException(string message, long personId) : base(message, personId)
        {
        }

        
    }
}
