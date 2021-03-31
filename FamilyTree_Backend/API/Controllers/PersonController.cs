using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.PersonModels;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Infrastructure.Service.InternalServices;
using FamilyTreeBackend.Infrastructure.Service.InternalServices.CustomException;
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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonController : BaseController
    {
        private readonly IPersonService _personService;
        public PersonController(UserManager<ApplicationUser> userManager, IPersonService personService) : base(userManager)
        {
            _personService = personService;
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestRoute()
        {
            return Ok("OwO");
        }

        [HttpGet("{personId}")]
        public async Task<IActionResult> FindPerson(long personId)
        {
            try
            {
                PersonModel personModel = await _personService.GetPerson(personId);
                return Ok(personModel);
            }
            catch(PersonNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { ex.Message, ex.StackTrace });
            }
        }

        [HttpGet("{personId}/children")]
        public async Task<IActionResult> FindChildren(long personId)
        {
            return Ok(await _personService.GetPersonChildren(personId));
        }

    }
}
