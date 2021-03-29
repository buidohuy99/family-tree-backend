using FamilyTreeBackend.Core.Domain.Entities;
using System;

namespace FamilyTreeBackend.Core.Application.DTOs
{
    public class MarriageDTO : RelationshipDTO
    {
        public MarriageDTO(Marriage input) : base(input)
        {
            DateOfMarriage = input.DateOfMarriage;
            EndOfMarriage = input.EndOfMarriage;
        }

        public DateTime DateOfMarriage { get; set; }
        public DateTime? EndOfMarriage { get; set; }
    }
}
