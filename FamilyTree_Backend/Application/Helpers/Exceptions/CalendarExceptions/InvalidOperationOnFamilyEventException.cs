using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.CalendarExceptions
{
    public class InvalidOperationOnFamilyEventException : CalendarException
    {
        public InvalidOperationOnFamilyEventException(string message, long eventId = 0) : base(message, eventId)
        {
        }
    }
}
