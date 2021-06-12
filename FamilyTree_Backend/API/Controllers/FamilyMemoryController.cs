using FamilyTreeBackend.Core.Application.Helpers;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.FamilyEvents;
using FamilyTreeBackend.Core.Application.Models.FamilyMemory;
using FamilyTreeBackend.Core.Application.Models.FamilyTree;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Presentation.API.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Controllers
{
    [Area("memory-management")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FamilyMemoryController : BaseController
    {
        private readonly IMemoryService _memoryService;
        private readonly ITreeAuthorizationService _authorizationService;
        public FamilyMemoryController(
            UserManager<ApplicationUser> userManager,
            IMemoryService memoryService,
            ITreeAuthorizationService authorizationService) : base(userManager)
        {
            _memoryService = memoryService;
            _authorizationService = authorizationService;
        }

        [HttpGet("memories/tree/{treeId}")]
        [SwaggerOperation(Summary = "Get all the memory in the tree with provided Id")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<IEnumerable<FamilyMemoryModel>>))]
        public async Task<IActionResult> GetMemoryByTree(long treeId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                treeId,
                MemoryOperations.Read
            );
            if (!authorizationResult.Succeeded)
            {
                return StatusCode(403, new HttpResponse<AuthorizationFailure>(
                    authorizationResult.Failure,
                    GenericResponseStrings.Tree_NoPermissionRead));
            }

            var result = await _memoryService.FindAllMemoriesOfTree(treeId);
            return Ok(new HttpResponse<IEnumerable<FamilyMemoryModel>>(result, GenericResponseStrings.MemoryControoler_FindMemoriesSuccesful));
        }
        
        [HttpPost("memory")]
        [SwaggerOperation(Summary = "Add new family memory")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<FamilyMemoryModel>))]
        public async Task<IActionResult> AddMemory([FromBody] FamilyMemoryInputModel input)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                input.FamilyTreeId,
                MemoryOperations.Create
            );
            if (!authorizationResult.Succeeded)
            {
                return StatusCode(403, new HttpResponse<AuthorizationFailure>(
                    authorizationResult.Failure,
                    GenericResponseStrings.Tree_NoPermissionEdit));
            }

            var result = await _memoryService.AddMemory(User, input);
            var response = new HttpResponse<FamilyMemoryModel>(
                result, 
                GenericResponseStrings.MemoryControoler_AddMemorySuccesful);
            return Ok(response);
        }

        [HttpDelete("memory/{memoryId}")]
        [SwaggerOperation(Summary = "Delete the memory with provided Id")]
        [SwaggerResponse(200, Type = typeof(string))]
        public async Task<IActionResult> AddMemory(long memoryId)
        {
            var authorizationResult = await _authorizationService.AuthorizeWithMemoryAsync(
                User,
                memoryId,
                MemoryOperations.Delete
            );
            if (!authorizationResult.Succeeded)
            {
                return StatusCode(403, new HttpResponse<AuthorizationFailure>(
                    authorizationResult.Failure,
                    GenericResponseStrings.Tree_NoPermissionEdit));
            }

            await _memoryService.DeleteMemory(memoryId);
            return Ok(GenericResponseStrings.MemoryControoler_DeleteMemorySuccesful);
        }
    }
}
