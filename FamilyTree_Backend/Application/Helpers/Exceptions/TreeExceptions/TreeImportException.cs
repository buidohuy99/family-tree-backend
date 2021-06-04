using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.TreeExceptions
{
    public class TreeImportException : TreeException
    {
        public TreeImportException(string message, long treeId) : base(message, treeId)
        {
        }
    }
}
