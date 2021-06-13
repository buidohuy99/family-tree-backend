using AutoMapper;
using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.MemoryExceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.FamilyMemory;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    public class MemoryService : IMemoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public MemoryService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<FamilyMemoryModel> AddMemory(ClaimsPrincipal creator, FamilyMemoryInputModel model)
        {
            var user = await _userManager.GetUserAsync(creator);
            if (user == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound);
            }
            var newMemory = _mapper.Map<FamilyMemory>(model);
            newMemory.CreatedBy = user;
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

            await _unitOfWork.Repository<FamilyMemory>().DeleteAsync(memoryId);
            await _unitOfWork.SaveChangesAsync();

            return;
        }

        public async Task<IEnumerable<FamilyMemoryModel>> FindAllMemoriesOfTree(long treeId)
        {
            var treeExists = await _unitOfWork.Repository<FamilyTree>().GetDbset()
                .AnyAsync(tr => tr.Id == treeId);
            if (!treeExists)
            {
                throw new TreeNotFoundException(TreeExceptionMessages.TreeNotFound, treeId);
            }

            var memories = await _unitOfWork.Repository<FamilyMemory>().GetDbset()
                .Include(m => m.CreatedBy)
                .Where(m => m.FamilyTreeId == treeId)
                .OrderByDescending(m => m.MemoryDate).ToListAsync();

            List<FamilyMemoryModel> result = new List<FamilyMemoryModel>();
            foreach(var memory in memories)
            {
                var item = _mapper.Map<FamilyMemoryModel>(memory);
                item.Creator = memory.CreatedByUserID != null ? new UserDTO(memory.CreatedBy) : null;
                result.Add(item);
            }
            return result;
        }
    }
}
