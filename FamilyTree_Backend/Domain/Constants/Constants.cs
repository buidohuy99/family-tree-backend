using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Domain.Constants
{
    public static class GenericResponseStrings
    {
        public const string AnExceptionOccuredInController = "A problem occurred when processing the content of your request";
        public const string LoginSuccessful = "You have logged in";
        public const string RegisterSuccessful = "You have successfully registered";
        public const string InternalServerError = "Server encountered an exception";
    }

    public static class LoggingMessages
    {
        public const string AuthService_ErrorMessage = "An error occured while processing an auth service function"; 
    }

    public static class AuthServiceExceptionMessages
    {
        public const string AuthService_CannotRegisterUser = "Cannot register the user with the above credentials";
        public const string AuthService_UsernameAlreadyExists = "Cannot register: username already exist";
        public const string AuthService_EmailAlreadyExists = "Cannot register: email already exist";
        public const string AuthService_CannotFindUser = "Valid user cannot be found from the specified infos";
        public const string AuthService_PasswordProvidedIsInvalid = "Provided password is wrong for the username/email";
    }
}
