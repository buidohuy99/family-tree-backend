using System;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions.MemoryExceptions
{
    [Serializable]
    public class FamilyMemoryNotFoundException : BaseServiceException
    {
        public long MemoryId { get; }
        public FamilyMemoryNotFoundException(string message, long memoryId = 0) : base(message)
        {
            MemoryId = memoryId;
        }
    }
}
