using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public class Person : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public Gender Gender { get; set; }
        public long? ChildOf { get; set; }
        public string Note { get; set; }
        public string UserId { get; set; }
        public Family ChildOfFamily { get; set; }

        public long FamilyTreeId { get; set; }

    }
}
