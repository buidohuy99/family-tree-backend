using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public class RequestResponseLog
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RequestPath { get; set; }
        public string Data { get; set; }
        public DateTime? DateCreated { get; set; }

    }
}
