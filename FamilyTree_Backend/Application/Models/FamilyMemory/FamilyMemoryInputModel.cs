﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FamilyTreeBackend.Core.Application.Models.FamilyMemory
{
    public class FamilyMemoryInputModel
    {
        [Required]
        public long FamilyTreeId { get; set; }
        public string Description { get; set; }
        public DateTime MemoryDate { get; set; }
        public ICollection<string> ImageUrls { get; set; }
    }
}
