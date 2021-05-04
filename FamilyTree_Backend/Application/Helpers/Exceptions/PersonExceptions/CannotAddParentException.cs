using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions
{
    public class CannotAddParentException : PersonException
    {
        public CannotAddParentException(string message, long personId)
            :base(message, personId)
        {
        }
    }
}
