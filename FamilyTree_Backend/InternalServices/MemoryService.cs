using AutoMapper;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.MemoryExceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.FamilyMemory;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    public class MemoryService : IMemoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public MemoryService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<FamilyMemoryModel> AddMemory(FamilyMemoryInputModel model)
        {
            var newMemory = _mapper.Map<FamilyMemory>(model);
            await _unitOfWork.Repository<FamilyMemory>().AddAsync(newMemory);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<FamilyMemoryModel>(newMemory);
            return result;
        }

        public async Task DeleteMemory(long memoryId)
        {
            var memory = await _unitOfWork.Repository<FamilyMemory>().FindAsync(memoryId);
            if (memory == null)
            {
                throw new FamilyMemoryNotFoundException
                    (MemoryExceptionMessages.FamilyMemoryNotFound, memoryId);
            }

            await _unitOfWork.Repository<FamilyEvent>().DeleteAsync(memoryId);
            await _unitOfWork.SaveChangesAsync();

            return;
        }
    }
}
