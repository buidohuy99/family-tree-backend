using System;
using System.Runtime.Serialization;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    [Serializable]
    public class PersonHasChildrenException : Exception
    {
        public PersonHasChildrenException()
        {
        }

        public PersonHasChildrenException(string message) : base(message)
        {
        }

        public PersonHasChildrenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PersonHasChildrenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}