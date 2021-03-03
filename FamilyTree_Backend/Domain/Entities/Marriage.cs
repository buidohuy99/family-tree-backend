using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeBackend.Core.Domain.Entities
{
    public class Marriage : Relationship
    {
        public DateTime DateOfMarriage { get; set; }
        public DateTime? EndOfMarriage { get; set; }
    }
}
