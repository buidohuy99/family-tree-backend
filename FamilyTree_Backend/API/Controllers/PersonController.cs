using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Controllers
{
    [Area("person-management")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonController : BaseController
    {
        protected PersonController(UserManager<ApplicationUser> userManager) : base(userManager)
        {
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestRoute()
        {
            return Ok("OwO");
        }
    }
}
