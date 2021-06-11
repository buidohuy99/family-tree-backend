using AutoMapper;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Operation.Models;
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
        public async Task<IEnumerable<RequestResponseListModel>> GetRequestResponseLogs(DateTime? from, DateTime? to, int page, int pageSize = 50)
        {
            if (from == null)
            {
                from = DateTime.Today;
            }
            if (to == null)
            {
                to = DateTime.Now;
            }

            var logs = await _unitOfWork.GetRequestResponseLogs()
                .Where(l => l.DateCreated >= from && l.DateCreated <= to)
                .AsNoTracking().Skip(page * pageSize).Take(pageSize)
                .OrderByDescending(l => l.DateCreated)
                .ToListAsync();

            var result = new List<RequestResponseListModel>();
            foreach(var log in logs)
            {
                result.Add(_mapper.Map<RequestResponseListModel>(log));
            }
            return result;
        }
    }
}
