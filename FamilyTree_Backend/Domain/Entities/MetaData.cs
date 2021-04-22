using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public interface MetaData
    {
        public DateTime? DateCreated { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
