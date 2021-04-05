using FamilyTreeBackend.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.Person
{
    public class AddNewChildToFamilyModel
    {
        public long? FatherId { get; set; }
        public long? MotherId { get; set; }
        [Required]
        public PersonInputModel ChildInfo { get; set; }
    }
}
