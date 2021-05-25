﻿using FamilyTreeBackend.Core.Application.Models.FamilyMemory;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IMemoryService
    {
        public Task<FamilyMemoryModel> AddMemory(FamilyMemoryInputModel model);
        public Task DeleteMemory(long memoryId);
    }
}