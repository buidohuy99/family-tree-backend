using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.CalendarExceptions
{
    public class FamilyEventRecurringDateException : FamilyEventDateException
    {
        public RepeatEvent EventRepeatType { get; set; }

        public FamilyEventRecurringDateException(string message, RepeatEvent repeatType, DateTime startDate, DateTime endDate, long eventId = 0) : base(message, startDate, endDate, eventId)
        {
            EventRepeatType = repeatType;
        }
    }
}
