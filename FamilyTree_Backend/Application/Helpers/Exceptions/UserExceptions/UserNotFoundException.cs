using FamilyTreeBackend.Core.Application.Helpers.Exceptions.UserExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions
{
    [Serializable]
    public class UserNotFoundException : UserException
    {
        public string Email { get; }
        public UserNotFoundException(string message, string userId = null, string email = null) :base(message, userId) 
        {
            Email = email;
        }
    }
}
