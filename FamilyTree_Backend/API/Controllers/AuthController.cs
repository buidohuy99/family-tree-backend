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
            // Carry on with the business logic
            AuthResponseModel result = await _authService.Login(model);

            return Ok(new HttpResponse<AuthResponseModel>(result, GenericResponseStrings.LoginSuccessful));
        }

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register new user")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<AuthResponseModel>))]
        public async Task<IActionResult> RegisterUser([FromBody] AuthRegisterModel model)
        {
            // Carry on with the business logic
            AuthResponseModel result = await _authService.RegisterUser(model);

            return Ok(new HttpResponse<AuthResponseModel>(result, GenericResponseStrings.RegisterSuccessful));
        }

        [HttpPost("refresh-access-token")]
        [SwaggerOperation(Summary = "Refresh the access token with a refresh token")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<RefreshTokenResponseModel>))]
        public async Task<IActionResult> RefreshAccessToken([FromBody] string refreshToken)
        {
            RefreshTokenResponseModel response = await _authService.RefreshAccessToken(refreshToken);
            return Ok(new HttpResponse<RefreshTokenResponseModel>(response, GenericResponseStrings.Auth_RefreshTokenSuccessful));
        }
    }
}
