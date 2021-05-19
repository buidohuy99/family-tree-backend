using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using System;
using System.Runtime.Serialization;

namespace FamilyTreeBackend.Core.Application.Helpers.Exceptions
{
    [Serializable]
    public class DeletePersonException : PersonException
    {
        public DeletePersonException(string message) : base(message) { }
        public DeletePersonException(string message, long personId) : base(message, personId)
        {
        }
    }
}