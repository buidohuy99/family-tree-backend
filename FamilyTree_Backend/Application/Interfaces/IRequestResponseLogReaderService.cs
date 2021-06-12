using FamilyTreeBackend.Core.Application.Operation.Models;
using System;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IRequestResponseLogReaderService
    {
        public Task<RequestResponsePageModel> GetRequestResponseLogs(DateTime? from, DateTime? to, uint page, uint pageSize = 50);
    }
}
