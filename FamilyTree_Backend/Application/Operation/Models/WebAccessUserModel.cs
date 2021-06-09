using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Operation.Models
{
    public class WebAccessUserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
    }
}
