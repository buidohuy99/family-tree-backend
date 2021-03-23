using FamilyTreeBackend.Core.Domain.Constants;
using System.Collections.Generic;

namespace FamilyTreeBackend.Presentation.API.Controllers.Misc
{
    public static class ServiceExceptionsProcessor
    {
        private static readonly Dictionary<string, uint> statusCodeDictionary;

        static ServiceExceptionsProcessor()
        {
            statusCodeDictionary = new Dictionary<string, uint>()
            {
                [AuthServiceExceptionMessages.AuthService_CannotRegisterUser] = 500,
                [AuthServiceExceptionMessages.AuthService_CannotFindUser] = 400,
                [AuthServiceExceptionMessages.AuthService_PasswordProvidedIsInvalid] = 400
            };
        }

        public static uint? GetStatusCode(string input)
        {
            if (input == null) return null;
            uint? result = statusCodeDictionary[input];
            return result;
        }
    }
}
