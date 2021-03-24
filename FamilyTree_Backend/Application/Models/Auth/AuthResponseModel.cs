using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.Auth
{
    public class AuthResponseModel
    {
        public UserDTO User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public AuthResponseModel(ApplicationUser user, string accessToken = null, string refreshToken = null)
        {
            if(user != null)
            {
                User = new UserDTO(user);
            }
            if(accessToken != null)
            {
                AccessToken = accessToken;
            }
            if(refreshToken != null)
            {
                RefreshToken = refreshToken;
            }
        }
    }
}
