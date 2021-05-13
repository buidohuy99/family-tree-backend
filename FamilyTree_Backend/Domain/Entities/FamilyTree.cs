using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public class FamilyTree : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }

        public ICollection<Person> People { get; set; }
        public ICollection<Family> Families { get; set; }
        public ApplicationUser Owner { get; set; }
        public ICollection<ApplicationUser> Editors { get; set; }
    }
}
