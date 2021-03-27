using FamilyTreeBackend.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.Person
{
    public class AddNewChildToPersonModel
    {
        [Required]
        public long PersonId { get; set; }
        public PersonDTO ChildInfo { get; set; }
    }
}
