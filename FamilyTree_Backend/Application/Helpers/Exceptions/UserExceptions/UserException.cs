using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.UserExceptions
{
    public class UserException : BaseServiceException
    {
        public string UserId { get; set; }
        public UserException(string message, string userId = null) : base(message)
        {
            UserId = userId;
        }
    }
}
