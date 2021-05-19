using FamilyTreeBackend.Core.Application.Helpers.Exceptions.TreeExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.CalendarExceptions
{
    [Serializable]
    public class FamilyEventNotFoundException : CalendarException
    {
        public FamilyEventNotFoundException(string message, long eventId = 0) : base(message, eventId)
        {
        }
    }
}
