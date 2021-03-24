using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace FamilyTreeBackend.Presentation.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[area]")]
    public class BaseController : ControllerBase
    {
        protected readonly UserManager<ApplicationUser> _userManager;

        protected BaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected string GetUserId(ClaimsPrincipal claimsManager)
        {
            if (claimsManager == null) return null;
            if (!claimsManager.HasClaim(c => c.Type == "uid"))
            {
                throw new Exception("Token provided is invalid because there is no valid confidential claim");
            }

            // Extract uid from token
            string uid;
            try
            {
                uid = claimsManager.Claims.FirstOrDefault(c => c.Type == "uid").Value;
            }
            catch (Exception)
            {
                throw new Exception("Token provided is invalid because the value for the claim is invalid");
            }

            // Check uid valid
            if (!_userManager.Users.Any(user => user.Id == uid))
            {
                throw new Exception("Token provided is invalid because the value for the claim is invalid");
            }

            return uid;
        }
    }
}
