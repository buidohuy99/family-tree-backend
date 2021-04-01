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
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public FamilyTreeDTO(FamilyTree tree)
        {
            if (tree == null) return;
            Id = tree.Id;
            Name = tree.Name;
            Description = tree.Description;
        }
    }
}
