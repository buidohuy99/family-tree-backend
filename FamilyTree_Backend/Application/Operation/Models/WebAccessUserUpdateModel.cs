using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Operation.Models
{
    public class WebAccessUserUpdateModel
    {
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Gender? Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

    }
}
