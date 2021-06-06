using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public class UserConnection
    {
        public long FamilyTreeId { get; set; }
        public string SourceUserId { get; set; }
        public string DestinationUserId { get; set; }
    }
}
