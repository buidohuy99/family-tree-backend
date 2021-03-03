using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public class FamilyTree : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<Person> People { get; set; }
        public IEnumerable<Family> Families { get; set; }
    }
}
