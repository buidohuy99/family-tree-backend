using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models
{
    public class PersonModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public long? Parent1Id { get; set; }
        public long? Parent2Id { get; set; }
        public Gender Gender { get; set; }
        public IEnumerable<PersonModel> Spouses;
        public string Note { get; set; }
        public string UserId { get; set; }
        public UserIconDTO ConnectedUser { get; set; }
        public string ImageUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string HomeAddress { get; set; }
        public string Occupation { get; set; }
    }
}
