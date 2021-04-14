using FamilyTreeBackend.Core.Application.Helpers.Exceptions.FamilyTreeService;
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
                [PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedUserFromId] = 400,
                [PersonServiceExceptionMessages.PersonService_CannotFindSpecifiedFamily] = 400,
                [PersonServiceExceptionMessages.PersonService_FamilyAlreadyExist] = 400,
                [PersonServiceExceptionMessages.PersonService_UserAlreadyExistedInTree] = 403,
                [PersonServiceExceptionMessages.PersonService_FatherGenderIsNotValid] = 400,
                [PersonServiceExceptionMessages.PersonService_MotherGenderIsNotValid] = 400,
                [PersonServiceExceptionMessages.PersonService_SpouseGenderNotValid] = 400,
                [PersonServiceExceptionMessages.PersonService_CannotAddChildToNoFamily] = 400,
                [PersonServiceExceptionMessages.PersonService_PersonNotFound] = 404,
                [PersonServiceExceptionMessages.PersonService_CannotDeletePerson] = 400,

                [nameof(TreeNotFoundException)] = 404,
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
