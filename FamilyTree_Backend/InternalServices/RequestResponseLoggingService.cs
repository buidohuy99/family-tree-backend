using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    public class RequestResponseLoggingService : IRequestResponseLoggingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RequestResponseLoggingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task SaveLogData(RequestResponseDataModel model)
        {
            var xmlStr = RequestResponseDataModel.GetXMLStringFromData(model);
            var log = new RequestResponseLog();
            log.Data = xmlStr;
            await _unitOfWork.GetRequestResponseLogs().AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
        }

        
    }
}
