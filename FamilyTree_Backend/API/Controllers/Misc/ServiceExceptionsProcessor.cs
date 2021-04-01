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
                [AuthServiceExceptionMessages.AuthService_PasswordProvidedIsInvalid] = 400,
                [AuthServiceExceptionMessages.AuthService_UsernameAlreadyExists] = 400,
                [AuthServiceExceptionMessages.AuthService_EmailAlreadyExists] = 400,

                [PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedPersonFromId] = 400,
                [PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedParentPersonFromId] = 400,
                [PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedTreeFromId] = 400,
                [PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedUserFromId] = 400,
                [PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedFamilyFromId] = 400,
                [PersonServiceExceptionMessages.PersonService_NoSlotForParentOfPerson] = 403,
                [PersonServiceExceptionMessages.PersonService_UserAlreadyExistedInTree] = 403,
                [PersonServiceExceptionMessages.PersonService_PersonCannotBeParentTwiceInAFamily] = 403,
                [PersonServiceExceptionMessages.PersonService_ParentCannotBeOneself] = 400,
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
