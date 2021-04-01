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

        // Person Controller
        public const string PersonController_AddPersonToTreeSuccessful = "You have successfully add a person to a tree";
        public const string PersonController_AddParentToPersonSuccessful = "You have successfully added a parent to the person below";

        public const string InternalServerError = "Server encountered an exception";
    }

    public static class LoggingMessages
    {
        public const string AuthService_ErrorMessage = "An error occured while processing an auth service function";
        public const string PersonService_ErrorMessage = "An error occured while processing a person service function";
    }

    public static class AuthServiceExceptionMessages
    {
        public const string AuthService_CannotRegisterUser = "Cannot register the user with the above credentials";
        public const string AuthService_UsernameAlreadyExists = "Cannot register: username already exist";
        public const string AuthService_EmailAlreadyExists = "Cannot register: email already exist";
        public const string AuthService_CannotFindUser = "Valid user cannot be found from the specified infos";
        public const string AuthService_PasswordProvidedIsInvalid = "Provided password is wrong for the username/email";
    }

    public static class PersonServiceExceptionMessages
    {
        public const string PersonService_CannotFindSpecifiedTreeFromId = "Tree cannot be found from the provided id";
        public const string PersonService_CannotFindSpecifiedUserFromId = "User cannot be found from the provided id";
        public const string PersonService_CannotFindSpecifiedPersonFromId = "Person cannot be found from the provided id";
        public const string PersonService_CannotFindSpecifiedParentPersonFromId = "Parent person cannot be found from the provided id";
        public const string PersonService_CannotFindSpecifiedFamilyFromId = "Family cannot be found from the provided id";
        public const string PersonService_UserAlreadyExistedInTree = "User already existed as a person in the tree";
        public const string PersonService_NoSlotForParentOfPerson = "No more parent slot for specified person";
        public const string PersonService_PersonCannotBeParentTwiceInAFamily = "Cannot set both parents of the family to be the same person";
        public const string PersonService_ParentCannotBeOneself = "Cannot set parent of the person to be himself";
    }
}
