using AutoMapper;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Operation.Models;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices.Operation
{
    public class RequestResponseLogReaderService : IRequestResponseLogReaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RequestResponseLogReaderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<RequestResponsePageModel> GetRequestResponseLogs(DateTime? from, DateTime? to, uint page, uint pageSize = 50)
        {
            if (from == null)
            {
                from = DateTime.Today;
            }
            if (to == null)
            {
                to = DateTime.Now;
            }

            var query = _unitOfWork.GetRequestResponseLogs()
                .Where(l => l.DateCreated >= from && l.DateCreated <= to);

            var pageCount = (uint)query.Count();

            if (page > pageCount && page > 1)
            {
                page = pageCount;
            }

            var logs = await _unitOfWork.GetRequestResponseLogs()
                .Where(l => l.DateCreated >= from && l.DateCreated <= to)
                .AsNoTracking().Skip((int)((page - 1) * pageSize)).Take((int)pageSize)
                .OrderByDescending(l => l.DateCreated)
                .ToListAsync();

            var result = new List<RequestResponseListModel>();
            foreach(var log in logs)
            {
                result.Add(_mapper.Map<RequestResponseListModel>(RequestResponseDataModel.GetDataFromXMLString(log.Data)));
            }

            var resultPage = new RequestResponsePageModel(result, pageCount, page, pageSize);
            
            return resultPage;
        }
    }
}
