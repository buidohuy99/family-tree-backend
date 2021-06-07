using FamilyTreeBackend.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models.User
{
    public class FindUsersPaginationResponseModel
    {
        public IEnumerable<UserDTO> Result { get; set; }
        public ulong TotalPages { get; set; }
        public ulong CurrentPage { get; set; }
        public ulong ItemsPerPage { get; set; }
    }
}
