﻿using FamilyTreeBackend.Core.Application.Interfaces;
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
            Description = "Return the info of tree with given Id)")]
        public async Task<IActionResult> DeleteFamilyTree(long treeId)
        {
            await _familyTreeService.DeleteFamilyTree(treeId);
            return Ok();
        }

        [HttpPost("tree")]
        public async Task<IActionResult> AddFamilyTree([FromBody] FamilyTreeInputModel model)
        {
            var result = await _familyTreeService.AddFamilyTree(model);

            return Ok(result);
        }

        [HttpGet("tree")]
        public async Task<IActionResult> FindAllTrees()
        {
            var result = await _familyTreeService.FindAllTree();

            return Ok(result);
        }
    }
}
