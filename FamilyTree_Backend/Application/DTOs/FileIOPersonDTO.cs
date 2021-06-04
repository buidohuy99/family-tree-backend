using FamilyTreeBackend.Core.Application.Models.Person;
using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.DTOs
{
    public class FileIOPersonDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public long? Parent1Id { get; set; }
        public long? Parent2Id { get; set; }
        public long? ChildOfCoupleId { get; set; }
        public Gender Gender { get; set; }
        public IEnumerable<FileIOSpouseDTO> Spouses { get; set; }
        public string Note { get; set; }
    }
}
