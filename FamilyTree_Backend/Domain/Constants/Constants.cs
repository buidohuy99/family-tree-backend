using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Domain.Constants
{
    public static class GenericResponseStrings
    {
        //Upload controller
        public const string UploadImageSuccessful = "Image is successfully uploaded";

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
        public const string PersonController_FindPersonDetailsSuccessful = "Person details has been found successfully";
        public const string PersonController_UpdatePersonDetailsSuccessful = "Person details has been updated successfully";

        //Family Tree Controller
        public const string TreeController_FindTreeSuccessful = "Tree has been found successfully";
        public const string TreeController_FindAllTreeSuccessful = "List of trees has been found successfully";
        public const string TreeController_AddTreeSuccessful = "Tree has been added successfully";
        public const string TreeController_ImportTreeSuccessful = "Tree has been imported successfully";
        public const string TreeController_UpdateTreeSuccessful = "Tree has been updated successfully";
        public const string TreeController_RemoveTreeSuccessful = "Tree has been removed successful";
        public const string TreeController_AddEditorsToTreeSuccessful = "Editors has been added to tree successfully";
        public const string TreeController_RemoveEditorsFromTreeSuccessful = "Editors has been removed from tree successfully";
        public const string TreeController_GetEditorsOfTreeSuccessful = "Editors of this tree has been fetched successfully";

        //Calendar Controller
        public const string CalendarController_FindEventsSuccessful = "List of events has been found successfully";
        public const string CalendarController_AddEventSuccessful = "Event has been added successfully";
        public const string CalendarController_UpdateEventSuccessful = "Event has been updated successfully";
        public const string CalendarController_RemoveEventSuccessful = "Event has been removed successfully";
        public const string CalendarController_RescheduleEventSuccessful = "Event has been rescheduled successfully";
        public const string CalendarController_CancelEventSuccessful = "Event has been cancelled successfully";

        //User Controller
        public const string UserController_FilterUsersSuccessful = "Users has been filtered successfully";
        public const string UserController_UpdateUserSuccessful = "Update user infos successful";
        public const string UserController_FetchUserSuccessful = "Fetched the user's info successfully";
        public const string GenerateResetPasswordUrlSuccessful = "Reset password url has been generated successfully";
        public const string UserController_GetNotificationSuccessul = "List of notification has been foud successfully";
        public const string UserController_ReadNotificationSuccessul = "Notification has been mark for read successfully";
        public const string UserController_RemoveNotificationSuccessul = "Notification has been removed successfully";
        public const string UserController_FindConnectionsSuccessul = "Connections between users have been found successfully";

        public const string RequestProcessingError = "Error occured while processing your request";
        public const string InternalServerError = "Server encountered an exception";

        //Memory Controller
        public const string MemoryControoler_AddMemorySuccesful = "Memory has been created successfully";
        public const string MemoryControoler_DeleteMemorySuccesful = "Memory has been deleted successfully";
        public const string MemoryControoler_FindMemoriesSuccesful = "List of memories has been found successfully";

        //Permission
        public const string Tree_NoPermissionEdit = "User does not have permission to edit this tree";
        public const string Tree_NoPermissionDelete = "User does not have permission delete this tree";
        public const string Tree_NoPermissionRead = "User does not have permission to see this tree";
        public const string Person_NoPermissionEdit = "User does not have permission to edit this person";
        public const string Person_NoPermissionDelete = "User does not have permission to delete this person";
        public const string Person_NoPermissionRead = "User does not have permission to see this person";
    }

    public static class GeneralExceptionMessages
    {
        public const string PageOutOfBounds = "Cannot fetch page because it's out of bounds";
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
        public const string DisabledUser = "user has been deactivated, contact support for more information";

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
        public const string FamilyAlreadyFull = "Family that this person is a child of is already full, cannot add parent";
        
        //GenderNotValid
        public const string FatherGenderIsNotValid = "Father for the operation is not a male";
        public const string MotherGenderIsNotValid = "Mother for the operation is not a female";
        public const string SpouseGenderNotValid = "Gender of the spouse is not valid, spouse gender must be the opposite of the person";
        
        public const string PersonNotFound = "Cannot find person with provided Id";
        
        public const string CannotDeletePerson = "Cannot delete person, please remove all the person's personal relationships before deleting them";
        public const string TreeDivergenceAfterDeletion = "Cannot delete person because the family diverges after this deletion";
        public const string CannotDeleteOnlyPersonInTree = "This person has no associations and is therefore the only person in this family tree and cannot be deleted";
    }

    public static class TreeExceptionMessages
    {
        public const string TreeNotFound = "Cannot find tree with provided Id";
        public const string TreeImportFileDoesNotHaveProperFormat = "Cannot import tree from file with wrong format, cannot retrieve content";
    }

    public static class UserExceptionMessages
    {
        public const string UserNotFound = "Cannot find user with provided Id";
        public const string ResetPasswordFail = "Reset password failed";
        public const string ConfirmEmailFail = "Confirm email failed";
        public const string UpdateUserFail = "Cannot update specified user because server encountered an error while saving";
        public const string NotificationNotFound = "Cannot find notification with provided id";
    }

    public static class SendEmailExceptionMessages
    {
        public const string SendEmailFailed = "Cannot send message to the provided email";
    }

    public static class UploadFileExceptionMessages
    {
        public const string UploadFileFailed = "File failed to be uploaded to the server";
        public const string UploadFileLimitExceeded = "File limit exceeded, please upload a smaller file";
    }

    public static class CalendarExceptionMessages
    {
        public const string FamilyEventNotFound = "Cannot find event with provided id";
        public const string MissingDateOnInput = "Cannot update event if one of the dates is missing";
        public const string CannotAddMultipleFollowingEventsToEvent = "One event can only have one and only one follow up event";
        public const string NonRepeatableEventCantHaveFollowingEvents = "Cannot add following event to a non repeatable event";
        public const string StartDateIsAfterEndDate = "Start date must be before end date";
        public const string StartDateAndEndDateIsNotWithinSameRepeatCycle = "Start date and end date of event must be within the same cycle for recurring events";
        public const string FollowingEventMustBeAfterEvent = "Cannot add a following event before the current event";
    }

    public static class MemoryExceptionMessages
    {
        public const string FamilyMemoryNotFound = "Cannot find memory with provided id";
    }
}
