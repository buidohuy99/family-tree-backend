using FamilyTreeBackend.Core.Domain.Enums;
using System;

namespace FamilyTreeBackend.Core.Application.Models.FamilyEvents
{
    public class FamilyEventInputModel
    {
        public long FamilyTreeId { get; set; }
        public string Note { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RepeatEvent Repeat { get; set; }
        public int ReminderOffest { get; set; }
    }
}