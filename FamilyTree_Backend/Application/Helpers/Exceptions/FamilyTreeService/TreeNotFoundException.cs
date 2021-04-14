using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.FamilyTreeService
{
    [Serializable]
    public class TreeNotFoundException : FamilyTreeServiceException
    {
        public TreeNotFoundException(string message) : base(message)
        {
        }
    }
}
