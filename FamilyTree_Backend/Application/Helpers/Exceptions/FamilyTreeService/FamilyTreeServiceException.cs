using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.FamilyTreeService
{
    public class FamilyTreeServiceException : BaseServiceException
    {
        public FamilyTreeServiceException(string message) : base(message)
        {
        }
    }
}
