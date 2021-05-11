using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions
{
    public class PersonServiceException : BaseServiceException
    {
        public PersonServiceException(string message) : base(message)
        {
        }
    }
}
