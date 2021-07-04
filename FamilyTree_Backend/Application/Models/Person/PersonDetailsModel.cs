using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.Person
{
    public class PersonDetailsModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public Gender Gender { get; set; }
        public PersonSummaryDTO Father { get; set; }
        public PersonSummaryDTO Mother { get; set; }
        public IEnumerable<SpouseDetailDTO> Spouses { get; set; }
        public IEnumerable<PersonSummaryDTO> Children { get; set; }
        public string PhoneNumber { get; set; }
        public string HomeAddress { get; set; }
        public string Occupation { get; set; }
        public string Note { get; set; }
        public string UserId { get; set; }
        public UserIconDTO ConnectedUser { get; set; }
        public string ImageUrl { get; set; }
    }
}
