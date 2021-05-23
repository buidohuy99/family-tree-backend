using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.Auth
{
    public class RefreshTokenResponseModel
    {
        /// <summary>If NewRefreshToken is not null => your current refresh token is expired, use this NewRefreshToken</summary>
        public string NewRefreshToken { get; set; }
        public string AccessToken { get; set; }
        public UserDTO User { get; set; }

        public RefreshTokenResponseModel(ApplicationUser user, string accessToken, string refreshToken = null)
        {
            if(user != null)
            {
                User = new UserDTO(user);
            }
            if (accessToken != null)
            {
                AccessToken = accessToken;
            }
            if (refreshToken != null)
            {
                NewRefreshToken = refreshToken;
            }
        }
    }
}
