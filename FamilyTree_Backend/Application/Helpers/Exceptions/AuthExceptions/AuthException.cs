using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions
{
    public class AuthException : BaseServiceException
    {
        public List<IdentityError> IdentityErrors { get; set; }

        public AuthException(string message, List<IdentityError> identityErrors = null) : base(message)
        {
            IdentityErrors = identityErrors;
        }
    }
}
