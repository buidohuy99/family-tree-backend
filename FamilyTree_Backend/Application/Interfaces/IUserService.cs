using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Models;
using FamilyTreeBackend.Core.Application.Models.User;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IUserService
    {
        public Task<string> GenerateResetPasswordUrl(string email);
        public Task<IdentityResult> ResetPasswordWithToken(ResetPasswordModel model);
        public IEnumerable<UserDTO> FindUser(UserFilterModel model);
        public FindUsersPaginationResponseModel FindUser(UserFilterModel model, PaginationModel paginationModel);
        public Task<UserDTO> UpdateUser(string updatedUserId, UpdateUserModel model);
        public Task<UserDTO> GetUser(string userId);
        public Task<IEnumerable<NotificationDTO>> GetNotifications(ApplicationUser user);
        public Task<NotificationDTO> ReadNotification(long notficationId);
        public Task<NotificationDTO> RemoveNotification(long notficationId);
        Task TestTriggerNotification();
        public Task<IEnumerable<UserConnectionDTO>> FindUserConnection(ClaimsPrincipal user, string searchingUserId);
        public Task<string> GenerateConfirmEmailUrl(string email);
        public Task<IdentityResult> ConfirmEmailWithToken(ConfirmEmailModel model);
    }
}
