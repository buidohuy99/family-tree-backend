using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions
{
    public class PersonException : BaseServiceException
    {
        public long? PersonId { get; set; }
        public PersonException(string message) : base(message) { }

        public PersonException(string message, long? personId) : this(message) {
            PersonId = personId;
        }
    }
}
