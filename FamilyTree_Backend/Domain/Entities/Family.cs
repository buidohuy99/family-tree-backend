using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public class Family : BaseEntity
    {
        public long? Parent1Id { get; set; }
        public long? Parent2Id { get; set; }

        public Person Parent1 { get; set; }
        public Person Parent2 { get; set; }
        public IEnumerable<Person> Children { get; set; }
        public Relationship Relationship { get; set; }

    }
}
