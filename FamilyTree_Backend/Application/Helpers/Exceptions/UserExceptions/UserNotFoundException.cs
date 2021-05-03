using FamilyTreeBackend.Core.Application.Helpers.Exceptions.UserExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions
{
    public class UserNotFoundException : UserException
    {
        public UserNotFoundException(string message, string userId = null) :base(message, userId) 
        {
        }
    }
}
