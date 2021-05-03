using FamilyTreeBackend.Core.Application.Helpers;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.Auth;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Presentation.API.Controllers.Misc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Controllers
{
    [Area("authentication")]
    public class AuthController : BaseController
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login to the system")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<AuthResponseModel>))]
        public async Task<IActionResult> Login([FromBody] AuthLoginModel model)
        {
            try
            {
                // Carry on with the business logic
                AuthResponseModel result = await _authService.Login(model);

                return Ok(new HttpResponse<AuthResponseModel>(result, GenericResponseStrings.LoginSuccessful));
            }
            catch (Exception ex)
            {
                string genericMessage = GenericResponseStrings.AnExceptionOccuredInController;
                if (ex is BaseServiceException exception)
                {
                    uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
                    if (statusCode != null && statusCode.HasValue)
                    {
                        if (ex is AuthException authException)
                        {
                            return StatusCode((int)statusCode.Value, new HttpResponse<string>(exception.Message, genericMessage, authException.IdentityErrors));
                        }
                        else
                        {
                            return StatusCode((int)statusCode.Value, new HttpResponse<string>(exception.Message, genericMessage));
                        }
                    }
                }
                return StatusCode(500, new HttpResponse<Exception>(ex, GenericResponseStrings.InternalServerError));
            }
        }

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register new user")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<AuthResponseModel>))]
        public async Task<IActionResult> RegisterUser([FromBody] AuthRegisterModel model)
        {
            try
            {
                // Carry on with the business logic
                AuthResponseModel result = await _authService.RegisterUser(model);

                return Ok(new HttpResponse<AuthResponseModel>(result, GenericResponseStrings.RegisterSuccessful));
            }
            catch (Exception ex)
            {
                string genericMessage = GenericResponseStrings.AnExceptionOccuredInController;
                if (ex is BaseServiceException exception)
                {
                    uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
                    if (statusCode != null && statusCode.HasValue)
                    {
                        if (ex is AuthException authException)
                        {
                            return StatusCode((int)statusCode.Value, new HttpResponse<string>(exception.Message, genericMessage, authException.IdentityErrors));
                        }
                        else
                        {
                            return StatusCode((int)statusCode.Value, new HttpResponse<string>(exception.Message, genericMessage));
                        }
                    }
                }
                return StatusCode(500, new HttpResponse<Exception>(ex, GenericResponseStrings.InternalServerError));
            }
        }
    }
}
