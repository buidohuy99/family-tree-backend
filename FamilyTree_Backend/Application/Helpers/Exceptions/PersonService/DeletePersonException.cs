using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using System;
using System.Runtime.Serialization;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    [Serializable]
    public class DeletePersonException : PersonServiceException
    {
        
        public DeletePersonException(string message) : base(message)
        {
        }

    }
}