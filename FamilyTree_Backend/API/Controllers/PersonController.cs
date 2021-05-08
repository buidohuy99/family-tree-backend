using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Helpers;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.Person;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Presentation.API.Controllers.Misc;
using FamilyTreeBackend.Core.Application.Models;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Infrastructure.Service.InternalServices;
using FamilyTreeBackend.Infrastructure.Service.InternalServices.CustomException;
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
    //Commented for testing
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonController : BaseController
    {
        private readonly IPersonService _personService;

        public PersonController(UserManager<ApplicationUser> userManager, IPersonService personService) : base(userManager)
        {
            _personService = personService;
        }

        [HttpPost("person/{personId}/parent")]
        [SwaggerOperation(Summary = "Add new parent to an existing person")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<AddNewParentToPersonResponseModel>), Description = "Return family with the new parent inside")]
        public async Task<IActionResult> AddNewParent(long personId, [FromBody] PersonInputModel input)
        {
            //MOVE THIS TO FUTURE PART WHERE IT WILL HANDLE GENERAL PERMISSION
            //Commented for testing

            //// Check validity of the request
            //var claimsManager = HttpContext.User;
            //string uid = null;
            //try
            //{
            //    uid = GetUserId(claimsManager);
            //}
            //catch (Exception e)
            //{
            //    return Unauthorized(e.Message);
            //}

            //if (uid == null)
            //{
            //    return Unauthorized("Unauthorized individuals cannot access this route");
            //}

            // Carry on with the business logic
            var model = new AddNewParentToPersonModel()
            {
                PersonId = personId,
                ParentInfo = input
            };
            var result = await _personService.AddNewParent(model);

            return Ok(new HttpResponse<AddNewParentToPersonResponseModel>(result, GenericResponseStrings.PersonController_AddParentToPersonSuccessful));
            
        }

        [HttpPost("person/{personId}/spouse")]
        [SwaggerOperation(Summary = "Add new spouse to an existing person")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<PersonDTO>), Description = "Returns the new spouse")]
        public async Task<IActionResult> AddNewSpouse(long personId, [FromBody] PersonInputModel input)
        {
           
            // Carry on with the business logic
            var model = new AddNewSpouseToPersonModel()
            {
                PersonId = personId,
                SpouseInfo = input
            };
            PersonDTO result = await _personService.AddNewSpouse(model);

            return Ok(new HttpResponse<PersonDTO>(result, GenericResponseStrings.PersonController_AddSpouseToPersonSuccessful));
           
        }

        [HttpPost("person/child")]
        [SwaggerOperation(Summary = "Add new child to a family with motherId and fatherId")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<AddNewChildToFamilyResponseModel>), Description = "Return the info of the new child, along with the new parent created on the spot (if have)")]
        public async Task<IActionResult> AddNewChild([FromBody] AddNewChildToFamilyModel input)
        {
            var result = await _personService.AddNewChild(input);

            return Ok(new HttpResponse<AddNewChildToFamilyResponseModel>(result, GenericResponseStrings.PersonController_AddChildToPersonSuccessful));
        }

        [HttpGet("person/{personId}")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<PersonModel>), 
            Description = "Return the info of the person")]
        public async Task<IActionResult> FindPerson(long personId)
        {
            PersonModel personModel = await _personService.GetPerson(personId);
            return Ok(new HttpResponse<PersonModel>(
                personModel, GenericResponseStrings.PersonController_FindPersonSuccessful));
        }

        [HttpGet("person/{personId}/children")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<IEnumerable<PersonModel>>), 
            Description = "Return the children of the person")]
        public async Task<IActionResult> FindChildren(long personId)
        {
            IEnumerable<PersonModel> result = await _personService.GetPersonChildren(personId);
            return Ok(new HttpResponse<IEnumerable<PersonModel>>(result, $"List of children belonging to person: {personId}"));
        }

        [HttpDelete("person/{personId}")]
        [SwaggerResponse(200, Type = typeof(string),
            Description = "Remove the person with given Id")]
        public async Task<IActionResult> RemovePerson(long personId)
        {
            await _personService.RemovePerson(personId);
            return Ok(GenericResponseStrings.PersonController_RemovePersonSuccessful);
        }

        [HttpPut("person/{personId}")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<PersonModel>), 
            Description = "Update the info of the person")]
        public async Task<IActionResult> UpdatePersonInfo(long personId, [FromBody] PersonInputModel updatedPersonModel)
        {

            PersonModel personModel = await _personService.UpdatePersonInfo(personId, updatedPersonModel);
            var response = new HttpResponse<PersonModel>(personModel, GenericResponseStrings.PersonController_UpdatePersonSuccessful);
            return Ok(response);
        }
        

    }
}
