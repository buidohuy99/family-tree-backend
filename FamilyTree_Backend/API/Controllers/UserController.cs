using FamilyTreeBackend.Core.Application.Helpers;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.UserExceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.User;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Controllers
{
    [Area("user-management")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        
        public UserController(IEmailService emailService, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _emailService = emailService;
        }

        [HttpPost("sendTestEmail")]
        [SwaggerOperation(Summary = "This is used to test sending email")]
        public async Task<IActionResult> TestSendEmail([FromBody] string email)
        {
            string body = "<div>Hello world</div>";
            await _emailService.SendEmailAsync(email, "Test email sending", body);

            return Ok();
        }

        [AllowAnonymous]
        [SwaggerOperation(Summary = "Generate and return a token required for resetting password")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<string>), Description = "Returns token")]
        public async Task<IActionResult> SendResetPassword([FromBody] string email)
        {
            var resetPasswordUrl = await _userService.GenerateResetPasswordUrl(email);
            return Ok(new HttpResponse<string>(resetPasswordUrl, GenericResponseStrings.GenerateResetPasswordUrlSuccessful));
        }

        [AllowAnonymous]
        [HttpPost("resetPassword")]
        [SwaggerOperation(Summary = "Reset password for the user with provided email, require reset password token")]
        [SwaggerResponse(200, Description = "Pasword changed successfully")]
        public  async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            IdentityResult result = await _userService.ResetPasswordWithToken(model);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                throw new ResetPasswordFailExcpetion(UserExceptionMessages.ResetPasswordFail, email: model.Email, errors: result.Errors);
            }
        }


    }
}
