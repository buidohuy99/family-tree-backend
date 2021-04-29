using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.FamilyTree;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTreeBackend.Core.Application.Helpers;
using FamilyTreeBackend.Presentation.API.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace FamilyTreeBackend.Presentation.API.Controllers
{
    [Area("tree-management")]
    [ApiController]
    public class FamilyTreeController : BaseController
    {
        private readonly IFamilyTreeService _familyTreeService;
        private readonly ITreeAuthorizationService _authorizationService;
        private readonly IAuthorizationService authorizationService1;
        public FamilyTreeController(
            UserManager<ApplicationUser> userManager, 
            IFamilyTreeService familyTreeService,
            ITreeAuthorizationService authorizationService) : base(userManager)
        {
            _familyTreeService = familyTreeService;
            _authorizationService = authorizationService;
        }

        [HttpGet("tree/{treeId}")]
        [SwaggerResponse(200, Type = typeof(FamilyTreeModel), 
            Description = "Return the info of tree with given Id)")]
        public async Task<IActionResult> FindFamilyTree(long treeId)
        {
            FamilyTreeModel result = await _familyTreeService.FindFamilyTree(treeId);
            return Ok(result);
        }

        [HttpPut("tree/{treeId}")]
        [SwaggerResponse(200, Type = typeof(FamilyTreeUpdateResponseModel),
            Description = "Return the info of tree with given Id)")]
        public async Task<IActionResult> UpdateFamilyTree(long treeId, [FromBody] FamilyTreeInputModel model)
        {
            var result = await _familyTreeService.UpdateFamilyTree(treeId, model);
            return Ok(result);
        }

        [HttpDelete("tree/{treeId}")]
        [SwaggerResponse(200, Type = typeof(Nullable),
            Description = "Delete tree with given Id)")]
        public async Task<IActionResult> DeleteFamilyTree(long treeId)
        {
            await _familyTreeService.DeleteFamilyTree(treeId);
            return Ok();
        }

        [HttpPost("tree")]
        public async Task<IActionResult> AddFamilyTree([FromBody] FamilyTreeInputModel model)
        {
            var claimManager = HttpContext.User;

            try
            {
                var user = await _userManager.GetUserAsync(claimManager);

                var result = await _familyTreeService.AddFamilyTree(model, user);

                return Ok(result);
            }
            catch (UserNotFoundException e)
            {
                //not implemented yet
                return StatusCode(401, new HttpResponse<Exception>(e, "User not found"));
            }
            
        }

        [HttpGet("tree")]
        public async Task<IActionResult> FindAllTrees()
        {
            var result = await _familyTreeService.FindAllTree();

            return Ok(result);
        }

        [HttpPost("tree/{treeId}/AddUsersToEditor")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<string>),
            Description = "Add list of users to be tree's editor, return the added users)")]
        public async Task<IActionResult> AddUsersToEditor(long treeId, [FromBody] List<string> userNames)
        {
            var authorizeResult = await _authorizationService.AuthorizeAsync(User, treeId, TreeCRUDOperations.Update);

            if (!authorizeResult.Succeeded)
            {
                return Unauthorized();
            }
            var result =  await _familyTreeService.AddUsersToEditor(userNames);
            return Ok(result);
        }
    }
}
