using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Operation.Models
{
    public class RequestResponseLogDetailsModel
    {
        [DisplayName("Log Id")]
        public string Id { get; set; }
        [DisplayName("Request Schema")]
        public string RequestSchema { get; set; }
        [DisplayName("User Agent")]
        public string UserAgent { get; set; }
        [DisplayName("User Id")]
        public string UserId { get; set; }
        [DisplayName("Request Host")]
        public string RequestHost { get; set; }
        [DisplayName("Request Method")]
        public string RequestMethod { get; set; }
        [DisplayName("Request Path")]
        public string RequestPath { get; set; }
        [DisplayName("Request Body")]
        public string RequestBody { get; set; }
        [DisplayName("Status Code")]
        public string StatusCode { get; set; }
        [DisplayName("Response Body")]
        public string ResponseBody { get; set; }
        [DisplayName("Date Created")]
        public string DateCreated { get; set; }
    }
}
