using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.FamilyTree
{
    public class RemoveEditorsModel
    {
        public IEnumerable<string> Usernames { get; set; }
    }
}
