using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.TreeExceptions
{
    public class TreeException : BaseServiceException
    {
        public long TreeId { get; }
        public TreeException(string message, long treeId)
            : base(message)
        {
            TreeId = treeId;
        }
    }
}
