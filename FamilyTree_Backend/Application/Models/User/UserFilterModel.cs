using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.User
{
    public class UserFilterModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? BornBefore { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
