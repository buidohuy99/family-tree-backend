using FamilyTreeBackend.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.DTOs
{
    public class FileIOSpouseDTO
    {
        public class FileIOSpouseRelationshipDTO{
            public RelationshipType? RelationshipType { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }
        public long? SpouseId { get; set; }
        public long CoupleId { get; set; }
        public FileIOSpouseRelationshipDTO RelationshipInfo { get; set; }
    }
}
