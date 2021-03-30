using FamilyTreeBackend.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.DTOs
{
    public class MarriageDTO : RelationshipDTO
    {
        public DateTime DateOfMarriage { get; set; }
        public DateTime? EndOfMarriage { get; set; }
        public MarriageDTO(Marriage marriage) : base(marriage)
        {
            DateOfMarriage = marriage.DateOfMarriage;
            EndOfMarriage = marriage.EndOfMarriage;
        }
    }
}
