using FamilyTreeBackend.Core.Application.Helpers.Exceptions.UserExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions
{
    public class UserExistsInTreeException : UserException
    {
        public long TreeId { get; set; }
        public UserExistsInTreeException(string message, string userId, long treeId)
            :base(message, userId)
        {
            TreeId = treeId;
        }
    }
}
