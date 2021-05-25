using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public class FamilyEventHistory : BaseEntity
    {
        public long FamilyEventId { get; set; }
        public DateTime PointInTime { get; set; }
        public string Note { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RepeatEvent Repeat { get; set; }
        public int? ReminderOffest { get; set; }
        public bool ApplyToFollowingEvents { get; set; }

        public FamilyEvent BaseFamilyEvent { get; set; }
    }
}
