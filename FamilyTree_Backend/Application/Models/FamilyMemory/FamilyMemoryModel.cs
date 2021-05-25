using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.FamilyMemory
{
    public class FamilyMemoryModel
    {
        public long Id { get; set; }
        public long FamilyTreeId { get; set; }
        public string Description { get; set; }
        public DateTime MemoryDate { get; set; }
        public ICollection<string> ImageUrls { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
