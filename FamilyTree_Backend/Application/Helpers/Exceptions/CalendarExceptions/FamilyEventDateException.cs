using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.CalendarExceptions
{
    public class FamilyEventDateException : CalendarException
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public FamilyEventDateException(string message, DateTime? startDate, DateTime? endDate, long eventId = 0) : base(message, eventId)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
