using FamilyTreeBackend.Core.Application.Operation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IRequestResponseLogReaderService
    {
        public Task<IEnumerable<RequestResponseListModel>> GetRequestResponseLogs(DateTime? from, DateTime? to, int page, int pageSize = 50);
    }
}
