using FamilyTreeBackend.Core.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace FamilyTreeBackend.Core.Application.Models.Person
{
    public class AddPersonToTreeModel
    {
        [Required]
        public long FamilyTreeId { get; set; }
        [Required]
        public PersonInputModel PersonInfo { get; set; }
    }
}
