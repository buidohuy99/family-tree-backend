using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FamilyTreeBackend.Core.Application.Models.FamilyEvents
{
    public class FamilyEventInputModel
    {
        [Required]
        public long FamilyTreeId { get; set; }
        public string Note { get; set; }
        public int ReminderOffest { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [DefaultValue(RepeatEvent.NONE)]
        public RepeatEvent Repeat { get; set; }
    }
}