using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.Person
{
    class ConnectedUserPersonModel
    {
        public long FamilyTreeId { get; set; }
        public string SourceUserId { get; set; }
        public string DestinationUserId { get; set; }
    }
}
