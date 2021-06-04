using FamilyTreeBackend.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.FileIO
{
    public class FamilyTreeFileIOModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool PublicMode { get; set; }
        public ICollection<FileIOPersonDTO> People { get; set; }
    }
}
