using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace FamilyTreeBackend.Core.Application.Models.Person
{
    public class PersonInputModel
    {
        [Required]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string UserId { get; set; }
        public string Note { get; set; }
    }
}
