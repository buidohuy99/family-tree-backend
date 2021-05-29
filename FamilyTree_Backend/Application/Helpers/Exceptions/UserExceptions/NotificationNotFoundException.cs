using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.UserExceptions
{
    [Serializable]
    public class NotificationNotFoundException : UserException
    {
        public long NotificationId { get; }
        public NotificationNotFoundException(string message, long notificationId = 0) : base(message)
        {
            NotificationId = notificationId;
        }
    }
}
