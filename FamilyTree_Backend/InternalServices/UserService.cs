using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.User;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public async Task<string> GenerateResetPasswordUrl(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound, email: email);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var clientSite = _configuration.GetValue<string>("ClientSite");

            var passwordResetUrl = new StringBuilder(clientSite)
                .Append("/resetPassword/")
                .Append(token).ToString();

            return passwordResetUrl;
        }

        public async Task<IdentityResult> ResetPasswordWithToken(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound, email: model.Email);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            return result;
        }

    }
}
