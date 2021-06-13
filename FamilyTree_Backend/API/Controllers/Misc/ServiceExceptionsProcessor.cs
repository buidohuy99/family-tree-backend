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
                [AuthExceptionMessages.InvalidRefreshToken] = 400,
                [AuthExceptionMessages.RefreshTokenIsCorrupted] = 400,

                [PersonExceptionMessages.FamilyNotFound] = 404,
                [PersonExceptionMessages.FamilyAlreadyFull] = 400,
                [PersonExceptionMessages.UserAlreadyExistedInTree] = 403,
                [PersonExceptionMessages.FatherGenderIsNotValid] = 400,
                [PersonExceptionMessages.MotherGenderIsNotValid] = 400,
                [PersonExceptionMessages.SpouseGenderNotValid] = 400,
                [PersonExceptionMessages.PersonNotFound] = 404,
                [PersonExceptionMessages.CannotDeletePerson] = 400,
                [PersonExceptionMessages.CannotDeleteOnlyPersonInTree] = 400,
                [PersonExceptionMessages.TreeDivergenceAfterDeletion] = 403,

                [TreeExceptionMessages.TreeNotFound] = 404,
                [TreeExceptionMessages.TreeImportFileDoesNotHaveProperFormat] = 400,

                [UserExceptionMessages.UserNotFound] = 404,
                [UserExceptionMessages.ResetPasswordFail] = 500,
                [UserExceptionMessages.ConfirmEmailFail] = 500,
                [UserExceptionMessages.ChangeEmailFail] = 500,
                [UserExceptionMessages.UpdateUserFail] = 500,
                [UserExceptionMessages.NotificationNotFound] = 404,

                [SendEmailExceptionMessages.SendEmailFailed] = 500,

                [UploadFileExceptionMessages.UploadFileLimitExceeded] = 400,

                [CalendarExceptionMessages.FamilyEventNotFound] = 404,
                [CalendarExceptionMessages.StartDateIsAfterEndDate] = 400,
                [CalendarExceptionMessages.StartDateAndEndDateIsNotWithinSameRepeatCycle] = 400,
                [CalendarExceptionMessages.CannotAddMultipleFollowingEventsToEvent] = 403,
                [CalendarExceptionMessages.MissingDateOnInput] = 400,
                [CalendarExceptionMessages.NonRepeatableEventCantHaveFollowingEvents] = 403,
                [CalendarExceptionMessages.FollowingEventMustBeAfterEvent] = 400,

                [MemoryExceptionMessages.FamilyMemoryNotFound] = 404,

                [GeneralExceptionMessages.PageOutOfBounds] = 400,
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
