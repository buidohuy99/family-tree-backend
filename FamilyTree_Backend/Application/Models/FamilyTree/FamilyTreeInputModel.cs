﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.FamilyTree
{
    public class FamilyTreeInputModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool PublicMode { get; set; }
    }
}
