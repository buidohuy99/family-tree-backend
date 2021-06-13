using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.User
{
    public class ConfirmEmailModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
