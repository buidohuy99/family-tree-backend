using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.CalendarExceptions
{
    public abstract class CalendarException : BaseServiceException
    {
        public long EventId { get; }
        public CalendarException(string message, long eventId = 0) : base(message)
        {
            EventId = eventId;
        }
    }
}
