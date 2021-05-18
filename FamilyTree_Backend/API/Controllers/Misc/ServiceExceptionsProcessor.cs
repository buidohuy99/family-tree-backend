using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
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
                [AuthExceptionMessages.RegisterUserFail] = 500,
                [AuthExceptionMessages.CannotFindUser] = 400,
                [AuthExceptionMessages.InvalidPassword] = 400,
                [AuthExceptionMessages.UsernameAlreadyExists] = 400,
                [AuthExceptionMessages.EmailAlreadyExists] = 400,

                [PersonExceptionMessages.FamilyNotFound] = 400,
                [PersonExceptionMessages.FamilyAlreadyFull] = 400,
                [PersonExceptionMessages.UserAlreadyExistedInTree] = 403,
                [PersonExceptionMessages.FatherGenderIsNotValid] = 400,
                [PersonExceptionMessages.MotherGenderIsNotValid] = 400,
                [PersonExceptionMessages.SpouseGenderNotValid] = 400,
                [PersonExceptionMessages.PersonNotFound] = 400,
                [PersonExceptionMessages.CannotDeletePerson] = 400,

                [TreeExceptionMessages.TreeNotFound] = 404,

                [UserExceptionMessages.UserNotFound] = 400,

                [SendEmailExceptionMessages.SendEmailFailed] = 500,
            };
        }

        public static uint? GetStatusCode(string input)
        {
            if (input == null) return null;

            if (statusCodeDictionary.ContainsKey(input))
            {
                uint? result = statusCodeDictionary[input];
                return result;
            }
            else
            {
                return null;
            }
            
        }
    }
}
