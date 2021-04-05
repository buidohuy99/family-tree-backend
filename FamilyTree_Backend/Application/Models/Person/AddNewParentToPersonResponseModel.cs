using FamilyTreeBackend.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.Person
{
    public class AddNewParentToPersonResponseModel
    {
        public PersonDTO Father { get; set; }
        public PersonDTO Mother { get; set; }
    }
}
