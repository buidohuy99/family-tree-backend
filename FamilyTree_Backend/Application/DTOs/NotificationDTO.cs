using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.DTOs
{
    public class NotificationDTO
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }
    }
}
