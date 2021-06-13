using FamilyTreeBackend.Core.Application.Models.FamilyMemory;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IMemoryService
    {
        public Task<FamilyMemoryModel> AddMemory(ClaimsPrincipal creator, FamilyMemoryInputModel model);
        public Task DeleteMemory(long memoryId);
        public Task<IEnumerable<FamilyMemoryModel>> FindAllMemoriesOfTree(long treeId);
    }
}