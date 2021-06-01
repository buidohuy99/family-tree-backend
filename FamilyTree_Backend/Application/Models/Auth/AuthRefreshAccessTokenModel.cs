using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.Auth
{
    public class AuthRefreshAccessTokenModel
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
