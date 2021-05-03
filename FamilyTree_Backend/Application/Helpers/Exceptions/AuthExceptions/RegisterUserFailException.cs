using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.AuthExceptions
{
    public class RegisterUserFailException : AuthException
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public RegisterUserFailException(string message, 
            List<IdentityError> identityErrors = null,
            string username = null,
            string email = null) : base(message, identityErrors)
        {
            Username = username;
            Email = email;
        }
    }
}
