using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public class FamilyEvent : BaseEntity
    {
        public long FamilyTreeId { get; set; }
        public string Note { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RepeatEvent Repeat { get; set; }
        public int ReminderOffest { get; set; }
        public long? ParentEventId { get; set; }

        public FamilyTree FamilyTree { get; set; }
        public FamilyEvent ParentEvent { get; set; }
        public FamilyEvent FollowingEvent { get; set; }
        public IEnumerable<FamilyEventExceptionCase> EventExceptions { get; set; }
    }
}
