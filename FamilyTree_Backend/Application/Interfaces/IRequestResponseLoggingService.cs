using FamilyTreeBackend.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IRequestResponseLoggingService
    {
        public RequestResponseDataModel Model { get; set; } 
        public Task SaveLogData(RequestResponseDataModel model);
    }
}
