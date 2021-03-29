using FamilyTreeBackend.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.DTOs
{
    public class FamilyTreeDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<PersonDTO> People { get; set; }
        public ICollection<FamilyDTO> Families { get; set; }

        public FamilyTreeDTO(FamilyTree tree)
        {
            Name = tree.Name;
            Description = tree.Description;

            if(tree.People != null) 
            {
                People = tree.People.Select(e => new PersonDTO(e)).ToList();
            }
            if (tree.Families != null) {
                Families = tree.Families.Select(e => new FamilyDTO(e)).ToList();
            }
        }
    }
}
