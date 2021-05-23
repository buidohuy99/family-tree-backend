using FamilyTreeBackend.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.FamilyTree
{
    public class FamilyTreeContributorsModel
    {
        public UserDTO Owner { get; set; }
        public List<UserDTO> Editors { get; set; }
    }
}
