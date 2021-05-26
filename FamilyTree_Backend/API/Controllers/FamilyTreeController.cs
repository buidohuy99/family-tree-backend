using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.FamilyTree;
using FamilyTreeBackend.Core.Domain.Constants;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace FamilyTreeBackend.Presentation.API.Controllers
{
    [Area("tree-management")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FamilyTreeController : BaseController
    {
        private readonly IFamilyTreeService _familyTreeService;
        private readonly ITreeAuthorizationService _authorizationService;
        public FamilyTreeController(
            UserManager<ApplicationUser> userManager, 
            IFamilyTreeService familyTreeService,
            ITreeAuthorizationService authorizationService) : base(userManager)
        {
            _familyTreeService = familyTreeService;
            _authorizationService = authorizationService;
        }

        [HttpGet("tree/{treeId}")]
        [SwaggerOperation(Summary = "Find family tree from Id")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<FamilyTreeModel>), 
            Description = "Return the info of tree with given Id)")]
        public async Task<IActionResult> FindFamilyTree(long treeId)
        {
            var authorizeResult = await _authorizationService.AuthorizeAsync(User, treeId, TreeOperations.Read);

            if (!authorizeResult.Succeeded)
            {
                return Unauthorized(new HttpResponse<AuthorizationFailure>(
                    authorizeResult.Failure,
                    GenericResponseStrings.Tree_NoPermissionRead));
            }
            FamilyTreeModel result = await _familyTreeService.FindFamilyTree(treeId);
            return Ok(new HttpResponse<FamilyTreeModel>(result, GenericResponseStrings.TreeController_FindTreeSuccessful));
        }

        [HttpPut("tree/{treeId}")]
        [SwaggerOperation(Summary = "Update family tree with new infos")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<FamilyTreeUpdateResponseModel>),
            Description = "Update the info of tree with given Id and new info)")]
        public async Task<IActionResult> UpdateFamilyTree(long treeId, [FromBody] FamilyTreeInputModel model)
        {
            var authorizeResult = await _authorizationService.AuthorizeAsync(User, treeId, TreeOperations.Update);

            if (!authorizeResult.Succeeded)
            {
                return Unauthorized(new HttpResponse<AuthorizationFailure>(
                    authorizeResult.Failure,
                    GenericResponseStrings.Tree_NoPermissionEdit));
            }

            var result = await _familyTreeService.UpdateFamilyTree(treeId, model);
            return Ok(new HttpResponse<FamilyTreeUpdateResponseModel>(
                result, GenericResponseStrings.TreeController_UpdateTreeSuccessful));
        }

        [HttpDelete("tree/{treeId}")]
        [SwaggerOperation(Summary = "Delete tree provided by an id")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<object>),
            Description = "Delete tree with given Id)")]
        public async Task<IActionResult> DeleteFamilyTree(long treeId)
        {
            var authorizeResult = await _authorizationService.AuthorizeAsync(User, treeId, TreeOperations.Delete);

            if (!authorizeResult.Succeeded)
            {
                return Unauthorized(new HttpResponse<AuthorizationFailure>(
                    authorizeResult.Failure,
                    GenericResponseStrings.Tree_NoPermissionDelete));
            }

            await _familyTreeService.DeleteFamilyTree(treeId);
            return Ok(new HttpResponse<object>(null, GenericResponseStrings.TreeController_RemoveTreeSuccessful));
        }

        [HttpPost("tree")]
        [SwaggerOperation(Summary = "Add a new family tree")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<FamilyTreeModel>),
            Description = "Add a new tree with given new info")]
        public async Task<IActionResult> AddFamilyTree([FromBody] FamilyTreeInputModel model)
        {
            var result = await _familyTreeService.AddFamilyTree(model, HttpContext.User);

            return Ok(new HttpResponse<FamilyTreeModel>(result, GenericResponseStrings.TreeController_AddTreeSuccessful));

        }

        [AllowAnonymous]
        [HttpGet("trees")]
        [SwaggerOperation(Summary = "Find all trees in the system")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<IEnumerable<FamilyTreeListItemModel>>),
            Description = "Find all trees")]
        public async Task<IActionResult> FindAllTrees()
        {
            var result = await _familyTreeService.FindAllTree();

            return Ok(new HttpResponse<IEnumerable<FamilyTreeListItemModel>>(
                result, GenericResponseStrings.TreeController_FindAllTreeSuccessful));
        }

        [AllowAnonymous]
        [HttpGet("trees-from-keyword")]
        [SwaggerOperation(Summary = "Find all trees in the system from a keyword")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<IEnumerable<FamilyTreeListItemModel>>),
            Description = "Find all trees with Name/Description fitting a query string")]
        public async Task<IActionResult> FindAllTreesFromKeyword([FromQuery] string q)
        {
            var result = await _familyTreeService.FindTreesFromKeyword(q != null ? q : string.Empty);

            return Ok(new HttpResponse<IEnumerable<FamilyTreeListItemModel>>(
                result, GenericResponseStrings.TreeController_FindAllTreeSuccessful));
        }

        [HttpGet("trees/list")]
        [SwaggerOperation(Summary = "Find all trees of user")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<IEnumerable<FamilyTreeListItemModel>>),
            Description = "Find all trees accessible to user")]
        public async Task<IActionResult> FindAllTreeAccessibleToUser()
        {
            var result = await _familyTreeService.FindAllTreeAccessibleToUser(User);

            return Ok(new HttpResponse<IEnumerable<FamilyTreeListItemModel>>(
                result, GenericResponseStrings.TreeController_FindAllTreeSuccessful));
        }

        [HttpGet("trees-from-keyword/list")]
        [SwaggerOperation(Summary = "Find all trees of user from keyword")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<IEnumerable<FamilyTreeListItemModel>>),
            Description = "Find all trees accessible to user with Name/Description fitting a query string")]
        public async Task<IActionResult> FindAllTreesFromKeywordAccessibleToUser([FromQuery] string q)
        {
            var result = await _familyTreeService.FindTreesFromKeywordAccessibleToUser(User, q != null ? q : string.Empty);

            return Ok(new HttpResponse<IEnumerable<FamilyTreeListItemModel>>(
                result, GenericResponseStrings.TreeController_FindAllTreeSuccessful));
        }

        [HttpPost("tree/{treeId}/add-users-to-editor")]
        [SwaggerOperation(Summary = "Add an editor to edit tree (only open to tree owner)")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<IEnumerable<string>>),
            Description = "Add list of users to be tree's editor, return the added users' username")]
        public async Task<IActionResult> AddUsersToEditor(long treeId, [FromBody] List<string> userNames)
        {
            var authorizeResult = await _authorizationService.AuthorizeAsync(User, treeId, TreeOperations.AddEditor);

            if (!authorizeResult.Succeeded)
            {
                return Unauthorized(new HttpResponse<AuthorizationFailure>(
                    authorizeResult.Failure,
                    GenericResponseStrings.Tree_NoPermissionEdit));
            }
            var result =  await _familyTreeService.AddUsersToEditor(treeId, userNames);
            return Ok(new HttpResponse<IEnumerable<string>>(result, GenericResponseStrings.TreeController_AddEditorsToTreeSuccessful));
        }

        [HttpPost("tree/{treeId}/remove-users-from-editor")]
        [SwaggerOperation(Summary = "Remove editor from tree (only open to tree owner)")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<IEnumerable<string>>),
            Description = "Remove list of users from tree's editor, return the removed users' username")]
        public async Task<IActionResult> RemoveUsersFromEditor(long treeId, [FromBody] List<string> userNames)
        {
            var authorizeResult = await _authorizationService.AuthorizeAsync(User, treeId, TreeOperations.RemoveEditor);

            if (!authorizeResult.Succeeded)
            {
                return Unauthorized(new HttpResponse<AuthorizationFailure>(
                    authorizeResult.Failure,
                    GenericResponseStrings.Tree_NoPermissionEdit));
            }
            var result = await _familyTreeService.RemoveUsersFromEditor(treeId, userNames);
            return Ok(new HttpResponse<IEnumerable<string>>(result, GenericResponseStrings.TreeController_RemoveEditorsFromTreeSuccessful));
        }

        [HttpGet("tree/{treeId}/editors")]
        [SwaggerOperation(Summary = "Get all contributors of the tree")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<FamilyTreeContributorsModel>),
            Description = "Get editors of a tree")]
        public async Task<IActionResult> RetrieveEditors(long treeId)
        {
            var result = await Task.Run(() => { 
                return _familyTreeService.GetTreeEditors(treeId); 
            });
            return Ok(new HttpResponse<FamilyTreeContributorsModel>(result, GenericResponseStrings.TreeController_GetEditorsOfTreeSuccessful));
        }
    }
}
