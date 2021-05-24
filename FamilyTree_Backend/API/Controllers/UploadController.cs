using FamilyTreeBackend.Core.Application.Helpers;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.FileUpload;
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
    [Area("file-upload")]
    public class UploadController : BaseController
    {
        private IUploadService _uploadService;

        public UploadController(UserManager<ApplicationUser> userManager, IUploadService uploadService) : base(userManager)
        {
            _uploadService = uploadService;
        }

        [HttpPost("image")]
        [SwaggerOperation(Summary = "Upload a single image (request must be type form-data)")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<string>), Description = "Returns image url")]
        public async Task<IActionResult> UploadImage([FromForm] UploadSingleFileModel input)
        {
            var result = await _uploadService.UploadSingleImage(input);
            return Ok(new HttpResponse<string>(result, GenericResponseStrings.UploadImageSuccessful));
        }

        [HttpPost("images")]
        [SwaggerOperation(Summary = "Upload a single image (request must be type form-data)")]
        [SwaggerResponse(200, Type = typeof(HttpResponse<string>), Description = "Returns image url")]
        public async Task<IActionResult> UploadImages([FromForm] UploadMutipleFilesModel input)
        {
            var result = await _uploadService.UploadMutipleImages(input);
            return Ok(new HttpResponse<IEnumerable<string>>(result, GenericResponseStrings.UploadImageSuccessful));
        }
    }
}
