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
            if (from == null || from.Value == DateTime.MinValue)
            {
                from = DateTime.Today;
            }
            if (to == null || to.Value == DateTime.MinValue)
            {
                to = DateTime.Now;
            }

            if (from > to)
            {
                from = to;
            }

            if (page <= 0)
            {
                page = 1;
            }
            
            var query = _unitOfWork.GetRequestResponseLogs()
                .Where(l => l.DateCreated.Value.CompareTo(from.Value.ToUniversalTime()) >= 0
                    && l.DateCreated.Value.CompareTo(to.Value.ToUniversalTime()) <= 0);

            var pageCount = (uint)MathF.Ceiling((float)(query.Count()) / (float)(pageSize));

            if (pageCount == 0)
            {
                page = 1;
            }

            if (page > pageCount && pageCount > 0)
            {
                page = pageCount;
            }

            var logs = await query.AsNoTracking()
                .Skip((int)((page - 1) * pageSize)).Take((int)pageSize)
                .OrderByDescending(l => l.DateCreated)
                .ToListAsync();

            var result = new List<RequestResponseListModel>();
            foreach(var log in logs)
            {
                var item = _mapper.Map<RequestResponseListModel>(RequestResponseDataModel.GetDataFromXMLString(log.Data));
                item.Id = log.Id;
                result.Add(item);
            }

            var resultPage = new RequestResponsePageModel()
            {
                From = from.Value,
                To = to.Value,
                TotalPages = pageCount,
                CurrentPage = page,
                ItemsPerPage = pageSize,
                List = result,
            };
            
            return resultPage;
        }

        public async Task<RequestResponseLogDetailsModel> GetRequestResponseLogById(string id)
        {
            var log = await _unitOfWork.GetRequestResponseLogs().FindAsync(id);
            var model = _mapper.Map<RequestResponseLogDetailsModel>(RequestResponseDataModel.GetDataFromXMLString(log.Data));
            model.Id = log.Id;
            return model;
        }
    }
}
