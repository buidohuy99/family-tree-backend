using FamilyTreeBackend.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.DTOs
{
    public class FamilyDTO
    {
        public PersonDTO Parent1 { get; set; }
        public PersonDTO Parent2 { get; set; }
        public RelationshipDTO Relationship { get; set; }

        public FamilyDTO(Person parent1, Person parent2, Relationship relationship)
        {
            if (parent1 != null)
            {
                Parent1 = new PersonDTO(parent1);
            }
            if (parent2 != null)
            {
                Parent2 = new PersonDTO(parent2);
            }
            if (relationship != null)
            {
                if (relationship is Marriage)
                {
                    Relationship = new MarriageDTO(relationship as Marriage);
                }
                else
                {
                    Relationship = new RelationshipDTO(relationship);
                }
            }
        }
    }
}
