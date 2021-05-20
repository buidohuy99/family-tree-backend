using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Models.User;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IUserService
    {
        public Task<string> GenerateResetPasswordUrl(string email);
        public Task<IdentityResult> ResetPasswordWithToken(ResetPasswordModel model);
        public IEnumerable<UserDTO> FindUser(UserFilterModel model);
        public Task<UserDTO> UpdateUser(string updatedUserId, UpdateUserModel model);
    }
}
