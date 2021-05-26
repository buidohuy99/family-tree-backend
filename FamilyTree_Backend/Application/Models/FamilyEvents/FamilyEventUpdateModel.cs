using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.FamilyEvents
{
    public class FamilyEventUpdateModel
    {
        public string Note { get; set; }
        public int? ReminderOffest { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public RepeatEvent? Repeat { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool ApplyToFollowingEvents { get; set; }
    }
}
