using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions
{
    public class PaginationException : BaseServiceException
    {
        public ulong Page { get; set; }
        public ulong ItemsPerPage { get; set; }
        public ulong TotalPages { get; set; }

        public PaginationException(string message, ulong page, ulong itemsPerPage, ulong totalpages) : base(message)
        {
            Page = page;
            ItemsPerPage = itemsPerPage;
            TotalPages = totalpages;
        }
    }
}
