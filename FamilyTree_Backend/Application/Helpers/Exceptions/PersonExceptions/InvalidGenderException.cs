using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions
{
    public class InvalidGenderException : PersonException
    {
        public Gender Gender { get; set; }
        public InvalidGenderException(string message, long personId, Gender gender)
            :base(message, personId)
        {
            Gender = gender;
        }
    }
}
