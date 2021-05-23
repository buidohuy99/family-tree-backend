using FamilyTreeBackend.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.FamilyTree
{
    public class FamilyTreeListItemModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UserIconDTO Owner { get; set; }
        public ICollection<UserIconDTO> Editors { get; set; }
    }
}
