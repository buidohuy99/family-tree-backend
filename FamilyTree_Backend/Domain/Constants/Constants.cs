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

        public const string UploadImageSuccessful = "Image is successfully uploaded";

        public const string GenerateResetPasswordUrlSuccessful = "Reset password url has been generated successfully";

        // Auth related
        public const string Auth_UserIsNotValid = "Cannot find valid user from claims provided";
        public const string LoginSuccessful = "You have logged in";
        public const string RegisterSuccessful = "You have successfully registered";
        public const string Auth_RefreshTokenSuccessful = "You have successfully obtained a new access token";

        // Person Controller
        public const string PersonController_AddParentToPersonSuccessful = "You have successfully added a parent to the person below, you can also find the family that the new parent belongs to below";
        public const string PersonController_AddSpouseToPersonSuccessful = "You have successfully added a spouse to the person and produced the below family";
        public const string PersonController_AddChildToPersonSuccessful = "You have successfully added a child to the person and the info of that child is below, along with the family he was added in";
        public const string PersonController_FindPersonSuccessful = "Person has been found successfully";
        public const string PersonController_UpdatePersonSuccessful = "Person has been updated successfully";
        public const string PersonController_RemovePersonSuccessful = "Person has been removed successfully";

        //Family Tree Controller
        public const string TreeController_FindTreeSuccessful = "Tree has been found successfully";
        public const string TreeController_FindAllTreeSuccessful = "List of trees has been found successfully";
        public const string TreeController_AddTreeSuccessful = "Tree has been added successfully";
        public const string TreeController_UpdateTreeSuccessful = "Tree has been updated successfully";
        public const string TreeController_RemoveTreeSuccessful = "Tree has been removed successful";
        public const string TreeController_AddEditorsToTreeSuccessful = "Editors has been added to tree successfully";
        public const string TreeController_NoPermissionToEditTree = "User doesn't have permission to edit tree";

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

        //RefreshTokenFail
        public const string InvalidRefreshToken = "Refresh token provided is either invalid or expired so please get a new one";
        public const string RefreshTokenIsCorrupted = "Refresh token is found to be corrupted please get a new one";
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
        public const string ResetPasswordFail = "Reset password failed";
    }

    public static class SendEmailExceptionMessages
    {
        public const string SendEmailFailed = "Cannot send message to the provided email";
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
