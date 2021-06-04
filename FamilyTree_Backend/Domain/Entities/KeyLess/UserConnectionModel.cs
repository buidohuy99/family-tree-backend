using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Domain.Entities.KeyLess
{
    public class UserConnectionModel : BaseEntity
    {
        public long FamilyTreeId { get; set; }
        public string SourceUserId { get; set; }
        public string DestinationUserId { get; set; }
    }
}
