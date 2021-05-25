using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.FileUpload
{
    /// <summary>This must be type form-data not from body</summary>
    public class UploadMutipleFilesModel
    {      
        /// <summary>
        /// File must be smaller than 2MB
        /// </summary>
        [Required]
        public IEnumerable<IFormFile> Files { get; set; }
    }
}
