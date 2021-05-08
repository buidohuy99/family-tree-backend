using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions
{
    public class FamilyNotFoundException : PersonException
    {
        public long FamilyId { get; set; }
        public FamilyNotFoundException(string message, long familyId) 
            :base(message)
        {
            FamilyId = familyId;
        }

        public FamilyNotFoundException(string message) : base(message)
        {
        }
    }
}
