using FamilyTreeBackend.Core.Application.Helpers.Misc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.FamilyTree
{
    public class FamilyTreeImportModel
    {
        [Required]
        [AllowedExtensions(new string[] { ".json"})]
        public IFormFile ImportedFile { get; set; }
    }
}
