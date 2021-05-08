using FamilyTreeBackend.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.Person
{
    public class AddNewChildToFamilyResponseModel
    {
        public PersonDTO NewChildInfo { get; set; }
        public PersonDTO NewFather { get; set; }
        public PersonDTO NewMother { get; set; }
    }
}
