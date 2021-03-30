using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public class Family : BaseEntity
    {
        public long? Parent1Id { get; set; }
        public long? Parent2Id { get; set; }
        public long FamilyTreeId { get; set; }

        public Person Parent1 { get; set; }
        public Person Parent2 { get; set; }
        public Relationship Relationship { get; set; }

        public FamilyTree FamilyTree { get; set; }
        public ICollection<Person> Children { get; set; }
    }
}
