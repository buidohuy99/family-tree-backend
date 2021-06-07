using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Models
{
    public class PaginationModel
    {
        [Required]
        public DateTime CreatedBefore { get; set; }
        [Required]
        [Range(1, ulong.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public ulong Page { get; set; }
        [Required]
        [Range(1, ulong.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public ulong ItemsPerPage { get; set; }
    }
}
