using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Helpers;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.UserExceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models;
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
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Controllers
{
    [Area("user-management")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [AllowAnonymous]
        [HttpPost("send-test-email")]
        [SwaggerOperation(Summary = "This is used to test sending email")]
        public async Task<IActionResult> TestSendEmail([FromBody] string email)
        {
            string body = "<div>Hello world</div>";
            await _emailService.SendEmailAsync(email, "Test email sending", body);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("reset-password-token")]
        [SwaggerOperation(Summary = "Generate and send a token required for resetting password to provided email")]
        [SwaggerResponse(200, Description = "Email has been successfully sent")]
        public async Task<IActionResult> SendResetPasswordToken([FromBody] ResetPasswordEmailInputModel input)
        {
            var resetPasswordUrl = await _userService.GenerateResetPasswordUrl(input.Email);
            await _emailService.SendResetPasswordEmail(input.Email, resetPasswordUrl);
            return Ok();
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

        [HttpPost("confirm-email-token")]
        [SwaggerOperation(Summary = "Generate and send a token required for confriming email to provided email")]
        [SwaggerResponse(200, Description = "Email has been successfully sent, no return response body")]
        public async Task<IActionResult> SendConfirmEmailToken([FromBody] ResetPasswordEmailInputModel input)
        {
            var confirmUrl = await _userService.GenerateConfirmEmailUrl(input.Email);
            await _emailService.SendEmailConfirmationEmail(input.Email, confirmUrl);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("confirm-email")]
        [SwaggerOperation(Summary = "confirm email for the user with provided email, require email confirmation token")]
        [SwaggerResponse(200, Description = "Email confirmed successfully")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailModel model)
        {
            IdentityResult result = await _userService.ConfirmEmailWithToken(model);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                throw new ConfirmEmailFailException(UserExceptionMessages.ConfirmEmailFail, email: model.Email, errors: result.Errors);
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

        [HttpPost("users/using-pagination")]
        [SwaggerOperation(Summary = "Filter users based on params, set params to null to not use that filter (only take a page)")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<FindUsersPaginationResponseModel>), Description = "Returns list of users in a page")]
        public async Task<IActionResult> FilterUsers([FromBody] UserFilterModel model, [FromQuery] PaginationModel paginationModel)
        {
            var result = await Task.Run(() => { return _userService.FindUser(model, paginationModel); });

            return Ok(new HttpResponse<FindUsersPaginationResponseModel>(result, GenericResponseStrings.UserController_FilterUsersSuccessful));
        }

        [HttpPut("user")]
        [SwaggerOperation(Summary = "Update info of a user")]
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

        [HttpGet("find-user-connection")]
        [SwaggerOperation(Summary = "Test trigger notification")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<IEnumerable<UserConnectionDTO>>),
            Description = "The list of connections, with each node in the list lead to another node (if any), eventually to the destination user, max 3 nodes deep")]
        public async Task<IActionResult> FindUserConnection([FromBody] string searchingUserId)
        {
            var result = await _userService.FindUserConnection(User, searchingUserId);
            return Ok(new HttpResponse<IEnumerable<UserConnectionDTO>>(result, GenericResponseStrings.UserController_FindConnectionsSuccessul));
        }
    }
}
