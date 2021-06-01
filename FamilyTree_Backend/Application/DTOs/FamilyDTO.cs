using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.DTOs
{
    public class FamilyDTO
    {
        public long Id { get; set; }
        public PersonDTO Parent1 { get; set; }
        public PersonDTO Parent2 { get; set; }
        public RelationshipDTO Relationship { get; set; }

        public FamilyDTO(Family family)
        {
            if (family == null) return;
            Id = family.Id;
            if (family.Parent1 != null)
            {
                Parent1 = new PersonDTO(family.Parent1);
            }
            if (family.Parent2 != null)
            {
                Parent2 = new PersonDTO(family.Parent2);
            }
            if (family.Relationship != null)
            {
                Relationship = new RelationshipDTO(family.Relationship);
            }
        }
    }
}
