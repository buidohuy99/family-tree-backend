using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using System;
using System.Runtime.Serialization;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    [Serializable]
    public class PersonHasChildrenException : PersonServiceException
    {
        
        public PersonHasChildrenException(string message) : base(message)
        {
        }

    }
}