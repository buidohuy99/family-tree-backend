using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.UserExceptions
{
    [Serializable]
    public class ConfirmEmailFailException : UserException
    {
        public ConfirmEmailFailException(string message, string userId = "", string email = "", IEnumerable<IdentityError> errors = null) : base(message, userId)
        {
            Email = email;
            IdentityErrors = errors;

        }

        public string Email { get; }
        public IEnumerable<IdentityError> IdentityErrors { get; set; }
    }
}
