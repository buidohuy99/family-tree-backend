using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Operation.Models
{
    public class WebAccessUserUpdateModel
    {
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string Address { get; set; }

    }
}
