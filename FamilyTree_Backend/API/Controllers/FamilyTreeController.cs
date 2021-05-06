using FamilyTreeBackend.Core.Application.Helpers;
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

namespace FamilyTreeBackend.Presentation.API.Controllers
{
    [Area("tree-management")]
    [ApiController]
    public class FamilyTreeController : BaseController
    {
        private readonly IFamilyTreeService _familyTreeService;
        public FamilyTreeController(UserManager<ApplicationUser> userManager, IFamilyTreeService familyTreeService) : base(userManager)
        {
            _familyTreeService = familyTreeService;
        }

        [HttpGet("tree/{treeId}")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<FamilyTreeModel>), 
            Description = "Return the info of tree with given Id)")]
        public async Task<IActionResult> FindFamilyTree(long treeId)
        {
            FamilyTreeModel result = await _familyTreeService.FindFamilyTree(treeId);
            return Ok(new HttpResponse<FamilyTreeModel>(result, GenericResponseStrings.TreeController_FindTreeSuccessful));
        }

        [HttpPut("tree/{treeId}")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<FamilyTreeUpdateResponseModel>),
            Description = "Update the info of tree with given Id and new info)")]
        public async Task<IActionResult> UpdateFamilyTree(long treeId, [FromBody] FamilyTreeInputModel model)
        {
            var result = await _familyTreeService.UpdateFamilyTree(treeId, model);
            return Ok(new HttpResponse<FamilyTreeUpdateResponseModel>(
                result, GenericResponseStrings.TreeController_UpdateTreeSuccessful));
        }

        [HttpDelete("tree/{treeId}")]
        [SwaggerResponse(200, Type = typeof(string),
            Description = "Remove the tree with given Id)")]
        public async Task<IActionResult> DeleteFamilyTree(long treeId)
        {
            await _familyTreeService.DeleteFamilyTree(treeId);
            return Ok(GenericResponseStrings.TreeController_RemoveTreeSuccessful);
        }

        [HttpPost("tree")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<FamilyTreeModel>),
            Description = "Add a new tree with given new info")]
        public async Task<IActionResult> AddFamilyTree([FromBody] FamilyTreeInputModel model)
        {
            var result = await _familyTreeService.AddFamilyTree(model);

            return Ok(new HttpResponse<FamilyTreeModel>(
                result, GenericResponseStrings.TreeController_AddTreeSuccessful));
        }

        [HttpGet("tree")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<IEnumerable<FamilyTreeListItemModel>>),
            Description = "Add a new tree with given new info")]
        public async Task<IActionResult> FindAllTrees()
        {
            var result = await _familyTreeService.FindAllTree();

            return Ok(new HttpResponse<IEnumerable<FamilyTreeListItemModel>>(
                result, GenericResponseStrings.TreeController_FindAllTreeSuccessful));
        }
    }
}
