using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public class Relationship : BaseEntity
    {
        public RelationshipType RelationshipType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Family Family { get; set; }
        
    }
}
