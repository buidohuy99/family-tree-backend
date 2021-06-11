using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Operation.Models
{
    public class RequestResponseListModel
    {
        public string DateCreated { get; set; }
        public string RequestHost { get; set; }
        public string RequestPath { get; set; }
        public string StatusCode { get; set; }
    }
}
