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

        public const string UploadImageSuccessful = "Image is successfully uploaded";

        // Person Controller
        public const string PersonController_AddParentToPersonSuccessful = "You have successfully added a parent to the person below, you can also find the family that the new parent belongs to below";
        public const string PersonController_AddSpouseToPersonSuccessful = "You have successfully added a spouse to the person and produced the below family";
        public const string PersonController_AddChildToPersonSuccessful = "You have successfully added a child to the person and the info of that child is below, along with the family he was added in";
        public const string PersonController_UpdatePersonSuccessful = "Person updated successfully";
        public const string PersonController_RemovePersonSuccessful = "Person removed successfully";

        public const string InternalServerError = "Server encountered an exception";
    }

    public static class LoggingMessages
    {
        public const string AuthService_ErrorMessage = "An error occured while processing an auth service function";
        public const string PersonService_ErrorMessage = "An error occured while processing a person service function";
        public const string UploadService_ErrorMessage = "An error occured while processing an upload service function";
    }

    public static class AuthExceptionMessages
    {
        //RegisterUserFail
        public const string RegisterUserFail = "Cannot register the user with the above credentials";
        public const string UsernameAlreadyExists = "Cannot register: username already exist";
        public const string EmailAlreadyExists = "Cannot register: email already exist";
        
        //LoginUserFail
        public const string CannotFindUser = "Valid user cannot be found from the specified infos";
        public const string InvalidPassword = "Provided password is wrong for the username/email";
    }

    public static class PersonExceptionMessages
    {
        //public const string PersonService_CannotFindSpecifiedParentPersonFromId = "Parent person cannot be found from the provided id";
        
        //FamilyNotFound
        public const string FamilyNotFound = "Family cannot be found for the person";
        public const string UserAlreadyExistedInTree = "User already existed as a person in the tree";
        public const string FamilyAlreadyExist = "Family that this person is a child of already exist, cannot add parent";
        
        //GenderNotValid
        public const string FatherGenderIsNotValid = "Father for the operation is not a male";
        public const string MotherGenderIsNotValid = "Mother for the operation is not a female";
        public const string SpouseGenderNotValid = "Gender of the spouse is not valid, spouse gender must be the opposite of the person";
        
        public const string PersonNotFound = "Cannot find person with provided Id";
        
        public const string CannotDeletePerson = "Cannot delete person, please remove all the person's personal relationships before deleting them";
    }

    public static class TreeExceptionMessages
    {
        public const string TreeNotFound = "Cannot find tree with provided Id";
    }

    public static class UserExceptionMessages
    {
        public const string UserNotFound = "Cannot find user with provided Id";
    }
}
