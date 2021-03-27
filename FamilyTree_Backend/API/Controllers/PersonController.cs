using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Models.Person;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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

        [HttpPost("person")]
        [SwaggerOperation(Summary = "Add new person to a family tree")]
        public async Task<IActionResult> AddNewPerson([FromBody] AddPersonToTreeModel input)
        {
            return null;
        }

        [HttpPost("person/{personId}/parent")]
        [SwaggerOperation(Summary = "Add new parent to an existing person")]
        public async Task<IActionResult> AddNewParent(long personId, [FromBody] PersonDTO input)
        {
            return null;
        }

        [HttpPost("person/{personId}/parent/{parentId}")]
        [SwaggerOperation(Summary = "Add an existing parent to an existing person")]
        public async Task<IActionResult> AddExistingParent(long personId, long parentId)
        {
            return null;
        }

        [HttpPost("person/{personId}/spouse")]
        [SwaggerOperation(Summary = "Add new spouse to an existing person")]
        public async Task<IActionResult> AddNewSpouse(long personId, [FromBody] PersonDTO input)
        {
            return null;
        }

        [HttpPost("person/{personId}/spouse/{spouseId}")]
        [SwaggerOperation(Summary = "Add an existing spouse to an existing person")]
        public async Task<IActionResult> AddExistingSpouse(long personId, long spouseId)
        {
            return null;
        }

        [HttpPost("person/{personId}/child")]
        [SwaggerOperation(Summary = "Add new child to an existing person")]
        public async Task<IActionResult> AddNewChild(long personId, [FromBody] PersonDTO input)
        {
            return null;
        }

        [HttpPost("person/{personId}/child/{childId}")]
        [SwaggerOperation(Summary = "Add an existing child to an existing person")]
        public async Task<IActionResult> AddExistingChild(long personId, long childId)
        {
            return null;
        }
    }
}
