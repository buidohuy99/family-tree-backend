using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Operation.Models
{
    public class RequestResponsePageModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string SearchUserId { get; set; }
        public uint TotalPages { get; set; }
        public uint CurrentPage { get; set; }
        public uint ItemsPerPage { get; set; }
        public IList<RequestResponseListModel> List { get; set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
               
    }
}
