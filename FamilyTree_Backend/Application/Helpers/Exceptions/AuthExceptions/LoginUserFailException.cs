using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.AuthExceptions
{
    [Serializable]
    public class LoginUserFailException : AuthException
    {
        public LoginUserFailException(string messgae) : base(messgae) { }
    }
}
