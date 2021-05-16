using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions
{
    public class BaseServiceException : Exception
    {
        public BaseServiceException(string message) : base(message)
        {
        }

        public BaseServiceException()
        {

        }
        public BaseServiceException(string message, Exception inner)
        : base(message, inner) { }

        
    }
}
