using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.CalendarExceptions
{
    public class FamilyEventHistoryNotFoundException : CalendarException
    {
        public long? EventHistoryId { get; set; }

        public FamilyEventHistoryNotFoundException(string message, long eventHistoryId, long eventId = 0) : base(message, eventId)
        {
            EventHistoryId = eventHistoryId;
        }
    }
}
