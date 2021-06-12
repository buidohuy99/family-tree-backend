using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Operation.Models
{
    public class RequestResponsePageModel : List<RequestResponseListModel>
    {
        public uint TotalPages { get; set; }
        public uint CurrentPage { get; set; }
        public uint ItemsPerPage { get; set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public RequestResponsePageModel(IEnumerable<RequestResponseListModel> list, uint totalPage, uint currentPage, uint itemsPerPage)
        {
            TotalPages = totalPage;
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            AddRange(list);
        }
    }
}
