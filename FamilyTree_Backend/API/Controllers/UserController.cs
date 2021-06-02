using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Helpers;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.UserExceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.User;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        public UserController(
            IEmailService emailService, 
            IUserService userService, 
            UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _emailService = emailService;
            _userService = userService;
        }

        [HttpPost("send-test-email")]
        [SwaggerOperation(Summary = "This is used to test sending email")]
        public async Task<IActionResult> TestSendEmail([FromBody] string email)
        {
            string body = "<div>Hello world</div>";
            await _emailService.SendEmailAsync(email, "Test email sending", body);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("retrieve-reset-password-url")]
        [SwaggerOperation(Summary = "Generate and return a token required for resetting password")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<string>), Description = "Returns token")]
        public async Task<IActionResult> SendResetPassword([FromBody] string email)
        {
            var resetPasswordUrl = await _userService.GenerateResetPasswordUrl(email);
            return Ok(new HttpResponse<string>(resetPasswordUrl, GenericResponseStrings.GenerateResetPasswordUrlSuccessful));
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
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

        [HttpPost("users")]
        [SwaggerOperation(Summary = "Filter users based on params, set params to null to not use that filter")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<IEnumerable<UserDTO>>), Description = "Returns list of users")]
        public async Task<IActionResult> FilterUsers([FromBody] UserFilterModel model)
        {
            var result = await Task.Run(() => { return _userService.FindUser(model); });

            return Ok(new HttpResponse<IEnumerable<UserDTO>>(result, GenericResponseStrings.UserController_FilterUsersSuccessful));
        }

        [HttpPut("user")]
        [SwaggerOperation(Summary = "Update info of a user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [SwaggerResponse(200, Type = typeof(HttpResponse<UserDTO>), Description = "Returns user info after update")]
        public async Task<IActionResult> UpdateUsers([FromBody] UpdateUserModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound);
            }

            var result = await _userService.UpdateUser(user.Id, model);

            return Ok(new HttpResponse<UserDTO>(result, GenericResponseStrings.UserController_UpdateUserSuccessful));
        }

        [HttpGet("user/{userId}")]
        [SwaggerOperation(Summary = "Get info of a specific user")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<UserDTO>), Description = "Returns a user's info")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var result = await _userService.GetUser(userId);

            return Ok(new HttpResponse<UserDTO>(result, GenericResponseStrings.UserController_FetchUserSuccessful));
        }

        [HttpGet("user-by-token")]
        [SwaggerOperation(Summary = "Get user info from a token")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [SwaggerResponse(200, Type = typeof(HttpResponse<UserDTO>), Description = "Returns a user's info")]
        public async Task<IActionResult> GetUserFromToken()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound);
            }

            return Ok(new HttpResponse<UserDTO>(new UserDTO(user), GenericResponseStrings.UserController_FetchUserSuccessful));
        }

        [HttpGet("notifications")]
        [SwaggerOperation(Summary = "Get user notifications")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [SwaggerResponse(200,Type = typeof(HttpResponse<IEnumerable<NotificationDTO>>))]
        public async Task<IActionResult> GetUserNotifications() 
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound);
            }

            var notifications = await _userService.GetNotifications(user);

            return Ok(new HttpResponse<IEnumerable<NotificationDTO>>(
                notifications, 
                GenericResponseStrings.UserController_GetNotificationSuccessul));
        }

        [HttpPut("notifications/{notficationId}")]
        [SwaggerOperation(Summary = "Mark notification as read")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [SwaggerResponse(200, Type = typeof(HttpResponse<NotificationDTO>))]
        public async Task<IActionResult> ReadNotification(long notficationId)
        {
            var noti = await _userService.ReadNotification(notficationId);

            return Ok(new HttpResponse<NotificationDTO>(
                noti,
                GenericResponseStrings.UserController_ReadNotificationSuccessul));
        }

        [HttpDelete("notifications/{notficationId}")]
        [SwaggerOperation(Summary = "Delete notification")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [SwaggerResponse(200, Type = typeof(string))]
        public async Task<IActionResult> RemoveNotification(long notficationId)
        {
            var noti = await _userService.RemoveNotification(notficationId);

            return Ok(GenericResponseStrings.UserController_RemoveNotificationSuccessul);
        }

        [HttpGet("notifications/test")]
        [SwaggerOperation(Summary = "Test trigger notification")]
        public async Task<IActionResult> TestTriggerNotification()
        {
            await _userService.TestTriggerNotification();

            return Ok();
        }
    }
}
