using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.FamilyEvents
{
    public class FamilyEventRescheduleModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public DateTime RescheduledStartDate { get; set; }
        [Required]
        public DateTime RescheduledEndDate { get; set; }
    }
}
