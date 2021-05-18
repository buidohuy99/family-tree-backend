using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.AuthExceptions
{
    public class RefreshTokenFailException : AuthException
    {
        public string RefreshToken { get; set; }

        public RefreshTokenFailException(string message, string refreshToken, List<IdentityError> identityErrors = null) : base(message, identityErrors)
        {
            RefreshToken = refreshToken;
        }
    }
}
