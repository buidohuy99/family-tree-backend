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

        public RelationshipDTO(Relationship relationship)
        {
            if (relationship == null) return;
            Id = relationship.Id;
            RelationshipType = relationship.RelationshipType;
        }
    }
}
