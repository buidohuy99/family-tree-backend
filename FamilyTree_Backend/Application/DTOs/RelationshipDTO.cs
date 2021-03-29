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
        public RelationshipType RelationshipType { get; set; }

        public FamilyDTO Family { get; set; }

        public RelationshipDTO(Relationship input)
        {
            RelationshipType = input.RelationshipType;
            if(input.Family != null)
            {
                Family = new FamilyDTO(input.Family);
            }
        }
    }
}
