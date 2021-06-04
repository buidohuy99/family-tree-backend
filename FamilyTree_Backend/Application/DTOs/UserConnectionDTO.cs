using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.DTOs
{
    public class UserConnectionDTO
    {
        public UserDTO ConnectionSource { get; set; }
        public UserDTO ConnectionDestination { get; set; }
        public FamilyTreeDTO FamilyTree { get; set; }
    }
}
