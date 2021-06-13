using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Operation.Models
{
    public class RequestResponseListModel
    {
        
        public string Id { get; set; }
        [DisplayName("Date Created")]
        public string DateCreated { get; set; }
        [DisplayName("User Agent")]
        public string UserAgent { get; set; }
        [DisplayName("User Id")]
        public string UserId { get; set; }
        [DisplayName("Request Path")]
        public string RequestPath { get; set; }
        [DisplayName("Status Code")]
        public string StatusCode { get; set; }
    }
}
