using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.UserExceptions
{
    [Serializable]
    public class ResetPasswordFailExcpetion : UserException
    {
        public ResetPasswordFailExcpetion(string message, string userId = null, string email = null, IEnumerable<IdentityError> errors = null) : base(message, userId)
        {
            Email = email;
            IdentityErrors = errors;
        }

        public string Email { get; set; }
        public IEnumerable<IdentityError> IdentityErrors { get; set; }
    }
}
