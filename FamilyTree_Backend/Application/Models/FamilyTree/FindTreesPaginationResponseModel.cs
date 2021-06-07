using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.FamilyTree
{
    public class FindTreesPaginationResponseModel
    {
        public IEnumerable<FamilyTreeListItemModel> Result { get; set; }
        public ulong TotalPages { get; set; }
        public ulong CurrentPage { get; set; }
        public ulong ItemsPerPage { get; set; }
    }
}
