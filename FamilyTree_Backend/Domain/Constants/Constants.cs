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
        public const string PersonController_AddParentToPersonSuccessful = "You have successfully added a parent to the person below, you can also find the family that the new parent belongs to below";
        public const string PersonController_AddSpouseToPersonSuccessful = "You have successfully added a spouse to the person and produced the below family";
        public const string PersonController_AddChildToPersonSuccessful = "You have successfully added a child to the person and the info of that child is below, along with the family he was added in";

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
        public const string PersonService_CannotFindSpecifiedFamily = "Family cannot be found for the person";
        public const string PersonService_UserAlreadyExistedInTree = "User already existed as a person in the tree";
        public const string PersonService_FamilyAlreadyExist = "Family that this person is a child of already exist, cannot add parent";
        public const string PersonService_FatherGenderIsNotValid = "Father for the operation is not a male";
        public const string PersonService_MotherGenderIsNotValid = "Mother for the operation is not a female";
        public const string PersonService_SpouseGenderNotValid = "Gender of the spouse is not valid, spouse gender must be the opposite of the person";
        public const string PersonService_CannotAddChildToNoFamily = "You have to specify a mother and/or a father to add the child to";
    }
}
