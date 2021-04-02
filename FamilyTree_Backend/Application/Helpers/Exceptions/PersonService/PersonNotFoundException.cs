using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices.CustomException
{
    [Serializable]
    public class PersonNotFoundException : PersonServiceException
    {
        public PersonNotFoundException(string message) : base(message)
        {
        }

        
    }
}
