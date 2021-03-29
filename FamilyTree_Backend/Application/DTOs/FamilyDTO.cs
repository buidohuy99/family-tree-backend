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
        public ICollection<PersonDTO> Children { get; set; }
        public RelationshipDTO Relationship { get; set; }
        public FamilyTreeDTO FamilyTree { get; set; }

        public FamilyDTO(Family input)
        {
            if(input.Parent1 != null)
            {
                Parent1 = new PersonDTO(input.Parent1);
            }
            if(input.Parent2 != null)
            {
                Parent2 = new PersonDTO(input.Parent2);
            }
            if(input.Children != null)
            {
                Children = input.Children.Select(e => new PersonDTO(e)).ToList();
            }
            if(input.FamilyTree != null)
            {
                FamilyTree = new FamilyTreeDTO(input.FamilyTree);
            }
            if(input.Relationship != null)
            {
                if(input.Relationship is Marriage)
                {
                    Relationship = new MarriageDTO(input.Relationship as Marriage);
                }
                else
                {
                    Relationship = new RelationshipDTO(input.Relationship);
                }
            }
        }
    }
}
