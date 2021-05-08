using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.DTOs
{
    public class PersonDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string HomeAddress { get; set; }
        public string Occupation { get; set; }
        public string Note { get; set; }

        public FamilyDTO ChildOfFamily { get; set; }
        public FamilyTreeDTO FamilyTree { get; set; }
        public UserDTO ConnectedUser { get; set; }

        public PersonDTO(Person person)
        {
            if (person == null) return;
            Id = person.Id;
            FirstName = person.FirstName;
            LastName = person.LastName;
            DateOfBirth = person.DateOfBirth;
            DateOfDeath = person.DateOfDeath;
            Gender = person.Gender;
            PhoneNumber = person.PhoneNumber;
            Occupation = person.Occupation;
            HomeAddress = person.HomeAddress;
            if (person.ChildOfFamily != null) 
            {
                ChildOfFamily = new FamilyDTO(person.ChildOfFamily);
            }
            if (person.FamilyTree != null)
            {
                FamilyTree = new FamilyTreeDTO(person.FamilyTree);
            }
            if(person.ConnectedUser != null)
            {
                ConnectedUser = new UserDTO(person.ConnectedUser);
            }
        }
    }
}
