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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public Gender Gender { get; set; }
        public string Note { get; set; }

        public long? ChildOfFamily { get; set; }
        public long FamilyTree { get; set; }
        public UserDTO ConnectedUser { get; set; }

        public PersonDTO(Person person)
        {
            if (person == null) return;
            FirstName = person.FirstName;
            LastName = person.LastName;
            DateOfBirth = person.DateOfBirth;
            DateOfDeath = person.DateOfDeath;
            Gender = person.Gender;
            ChildOfFamily = person.ChildOf;
            FamilyTree = person.FamilyTreeId;  
            if(person.ConnectedUser != null)
            {
                ConnectedUser = new UserDTO(person.ConnectedUser);
            }
        }
    }
}
