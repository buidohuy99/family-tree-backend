using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.DTOs
{
    public class SpouseDetailDTO
    {
        public PersonSummaryDTO PersonSummary;

        public RelationshipDTO Relationship;

        public SpouseDetailDTO(PersonSummaryDTO personSummary, RelationshipDTO relationship)
        {
            PersonSummary = personSummary;
            Relationship = relationship;
        }
    }
}
