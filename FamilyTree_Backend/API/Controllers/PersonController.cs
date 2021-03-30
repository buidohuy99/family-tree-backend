using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Helpers;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.Person;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Presentation.API.Controllers.Misc;
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
        private readonly IPersonService _personService;

        public PersonController(UserManager<ApplicationUser> userManager, IPersonService personService) : base(userManager)
        {
            _personService = personService;
        }

        [HttpPost("person")]
        [SwaggerOperation(Summary = "Add new person to a family tree")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<PersonDTO>))]
        public async Task<IActionResult> AddNewPerson([FromBody] AddPersonToTreeModel input)
        {
            try
            {
                // Check validity of the request
                var claimsManager = HttpContext.User;
                string uid = null;
                try
                {
                    uid = GetUserId(claimsManager);
                }
                catch (Exception e)
                {
                    return Unauthorized(e.Message);
                }

                if (uid == null)
                {
                    return Unauthorized("Unauthorized individuals cannot access this route");
                }

                // Carry on with the business logic
                PersonDTO result = await _personService.AddNewPerson(uid, input);

                return Ok(new HttpResponse<PersonDTO>(result, GenericResponseStrings.PersonController_AddPersonToTreeSuccessful));
            }
            catch (Exception ex)
            {
                string genericMessage = GenericResponseStrings.AnExceptionOccuredInController;
                if (ex is BaseServiceException exception)
                {
                    uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
                    if (statusCode != null && statusCode.HasValue)
                    {
                        return StatusCode((int)statusCode.Value, new HttpResponse<string>(exception.Message, genericMessage));
                    }
                }
                return StatusCode(500, new HttpResponse<Exception>(ex, GenericResponseStrings.InternalServerError));
            }
        }

        [HttpPost("person/{personId}/parent")]
        [SwaggerOperation(Summary = "Add new parent to an existing person")]
        public async Task<IActionResult> AddNewParent(long personId, [FromBody] PersonInputModel input)
        {
            try
            {
                // Check validity of the request
                var claimsManager = HttpContext.User;
                string uid = null;
                try
                {
                    uid = GetUserId(claimsManager);
                }
                catch (Exception e)
                {
                    return Unauthorized(e.Message);
                }

                if (uid == null)
                {
                    return Unauthorized("Unauthorized individuals cannot access this route");
                }

                // Carry on with the business logic
                var model = new AddNewParentToPersonModel()
                {
                    PersonId = personId,
                    ParentInfo = input
                };
                PersonDTO result = await _personService.AddNewParent(uid, model);

                return Ok(new HttpResponse<PersonDTO>(result, GenericResponseStrings.PersonController_AddParentToPersonSuccessful));
            }
            catch (Exception ex)
            {
                string genericMessage = GenericResponseStrings.AnExceptionOccuredInController;
                if (ex is BaseServiceException exception)
                {
                    uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
                    if (statusCode != null && statusCode.HasValue)
                    {
                        return StatusCode((int)statusCode.Value, new HttpResponse<string>(exception.Message, genericMessage));
                    }
                }
                return StatusCode(500, new HttpResponse<Exception>(ex, GenericResponseStrings.InternalServerError));
            }
        }

        [HttpPost("person/{personId}/parent/{parentId}")]
        [SwaggerOperation(Summary = "Add an existing parent to an existing person")]
        public async Task<IActionResult> AddExistingParent(long personId, [SwaggerParameter(Description = "Parent must not be in any family, families must be deleted before node could be used in this function")]long parentId)
        {
            try
            {
                // Check validity of the request
                var claimsManager = HttpContext.User;
                string uid = null;
                try
                {
                    uid = GetUserId(claimsManager);
                }
                catch (Exception e)
                {
                    return Unauthorized(e.Message);
                }

                if (uid == null)
                {
                    return Unauthorized("Unauthorized individuals cannot access this route");
                }

                // Carry on with the business logic
                PersonDTO result = await _personService.AddExistingParent(uid, personId, parentId);

                return Ok(new HttpResponse<PersonDTO>(result, GenericResponseStrings.PersonController_AddParentToPersonSuccessful));
            }
            catch (Exception ex)
            {
                string genericMessage = GenericResponseStrings.AnExceptionOccuredInController;
                if (ex is BaseServiceException exception)
                {
                    uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
                    if (statusCode != null && statusCode.HasValue)
                    {
                        return StatusCode((int)statusCode.Value, new HttpResponse<string>(exception.Message, genericMessage));
                    }
                }
                return StatusCode(500, new HttpResponse<Exception>(ex, GenericResponseStrings.InternalServerError));
            }
        }

        [HttpPost("person/{personId}/spouse")]
        [SwaggerOperation(Summary = "Add new spouse to an existing person")]
        public async Task<IActionResult> AddNewSpouse(string userPerformingCreation, long personId, [FromBody] PersonInputModel input)
        {
            return null;
        }

        [HttpPost("person/{personId}/spouse/{spouseId}")]
        [SwaggerOperation(Summary = "Add an existing spouse to an existing person")]
        public async Task<IActionResult> AddExistingSpouse(string userPerformingCreation, long personId, long spouseId)
        {
            return null;
        }

        [HttpPost("person/{personId}/child")]
        [SwaggerOperation(Summary = "Add new child to an existing person")]
        public async Task<IActionResult> AddNewChild(string userPerformingCreation, long personId, [FromBody] PersonInputModel input)
        {
            return null;
        }

        [HttpPost("person/{personId}/child/{childId}")]
        [SwaggerOperation(Summary = "Add an existing child to an existing person")]
        public async Task<IActionResult> AddExistingChild(string userPerformingCreation, long personId, long childId)
        {
            return null;
        }
    }
}
