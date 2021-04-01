using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Helpers;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.Person;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Presentation.API.Controllers.Misc;
using FamilyTreeBackend.Core.Application.Models.PersonModels;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonController : BaseController
    {
        private readonly IPersonService _personService;

        public PersonController(UserManager<ApplicationUser> userManager, IPersonService personService) : base(userManager)
        {
            _personService = personService;
        }

        [HttpPost("person/{personId}/parent")]
        [SwaggerOperation(Summary = "Add new parent to an existing person")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<PersonDTO>), Description = "Return the new info of the existing person, along with the family that houses his new parent")]
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

        [HttpPost("person/{personId}/spouse")]
        [SwaggerOperation(Summary = "Add new spouse to an existing person")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<FamilyDTO>), Description = "Returns the family created with the new spouse")]
        public async Task<IActionResult> AddNewSpouse(long personId, [FromBody] PersonInputModel input)
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
                var model = new AddNewSpouseToPersonModel()
                {
                    PersonId = personId,
                    SpouseInfo = input
                };
                FamilyDTO result = await _personService.AddNewSpouse(uid, model);

                return Ok(new HttpResponse<FamilyDTO>(result, GenericResponseStrings.PersonController_AddSpouseToPersonSuccessful));
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

        [HttpPost("person/{personId}/child")]
        [SwaggerOperation(Summary = "Add new child to an existing person")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<PersonDTO>), Description = "Return the info of the new child, along with the family that the child belongs to")]
        public async Task<IActionResult> AddNewChild(long personId, [FromBody] PersonInputModel input)
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
                var model = new AddNewChildToPersonModel()
                {
                    PersonId = personId,
                    ChildInfo = input
                };
                PersonDTO result = await _personService.AddNewChild(uid, model);

                return Ok(new HttpResponse<PersonDTO>(result, GenericResponseStrings.PersonController_AddChildToPersonSuccessful));
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

        [HttpGet("person/{personId}")]
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

        [HttpGet("person/{personId}/children")]
        public async Task<IActionResult> FindChildren(long personId)
        {
            return Ok(await _personService.GetPersonChildren(personId));
        }

    }
}
