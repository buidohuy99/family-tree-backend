using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public class FamilyMemory : BaseEntity
    {
        public long FamilyTreeId { get; set; }
        public string Description { get; set; }
        public DateTime MemoryDate { get; set; }
        public ICollection<string> ImageUrls { get; set; }

        public string CreatedByUserID { get; set; }
        public ApplicationUser CreatedBy { get; set; }
    }
}
