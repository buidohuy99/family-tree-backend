using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.DTOs
{
    public class RelationshipDTO
    {
        public long Id { get; set; }
        public RelationshipType RelationshipType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public RelationshipDTO() { }
        public RelationshipDTO(Relationship relationship)
        {
            if (relationship == null) return;
            Id = relationship.Id;
            RelationshipType = relationship.RelationshipType;
            StartDate = relationship.StartDate;
            EndDate = relationship.EndDate;
        }
    }
}
