using AutoMapper;
using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.UserExceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.User;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Core.Domain.Entities.KeyLess;
using FamilyTreeBackend.Infrastructure.Persistence.Repository;
using FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.Quartz.QuartzJobs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StopWord;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(
            UserManager<ApplicationUser> userManager, 
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<string> GenerateResetPasswordUrl(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound, email: email);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var clientSite = _configuration.GetValue<string>("ClientSite");

            var passwordResetUrl = new StringBuilder(clientSite)
                .Append("/resetPassword/")
                .Append(token).ToString();

            return passwordResetUrl;
        }

        public async Task<IdentityResult> ResetPasswordWithToken(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound, email: model.Email);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            return result;
        }

        public IEnumerable<UserDTO> FindUser(UserFilterModel model)
        {
            var users = _userManager.Users;

            if (model.UserName != null)
            {
                users = users.Where(e => e.UserName.Equals(model.UserName));
            }
            else if (model.Email != null)
            {
                users = users.Where(e => e.Email.Equals(model.Email));
            }

            if (model.UserName == null && model.Email == null)
            {
                if (model.Phone != null)
                {
                    users = users.Where(e => e.PhoneNumber.Contains(model.Phone));
                }

                if (model.BornBefore.HasValue)
                {
                    users = users.Where(e => (e.DateOfBirth.HasValue ? e.DateOfBirth.Value.CompareTo(model.BornBefore.Value) : int.MaxValue) < 0);
                }

                if (model.Gender.HasValue)
                {
                    users = users.Where(e => e.Gender == model.Gender);
                }

                if (model.Name != null)
                {
                    var nameWithoutStopwords = model.Name.RemoveStopWords().ToLower();

                    MatchCollection matches = Regex.Matches(nameWithoutStopwords, "[a-z]([:'-]?[a-z])*",
                                        RegexOptions.IgnoreCase);
                    foreach (Match match in matches)
                    {
                        users = users.Where(e => e.FirstName.ToLower().Contains(match.Value)
                                || e.LastName.ToLower().Contains(match.Value) || e.MidName.ToLower().Contains(match.Value));
                    }
                }
            }

            return users.Select(e => new UserDTO(e));
        }

        public async Task<UserDTO> UpdateUser(string updatedUserId, UpdateUserModel model)
        {
            var user = await _userManager.FindByIdAsync(updatedUserId);

            if(user == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound, updatedUserId);
            }

            if(model.FirstName != null)
            {
                user.FirstName = string.IsNullOrEmpty(model.FirstName) ? null : model.FirstName;
            }

            if(model.MidName != null)
            {
                user.MidName = string.IsNullOrEmpty(model.MidName) ? null : model.MidName;
            }

            if(model.LastName != null)
            {
                user.LastName = string.IsNullOrEmpty(model.LastName) ? null : model.LastName;
            }

            if(model.Address != null)
            {
                user.Address = string.IsNullOrEmpty(model.Address) ? null : model.Address;
            }

            if(model.AvatarUrl != null)
            {
                user.AvatarUrl = string.IsNullOrEmpty(model.AvatarUrl) ? null : model.AvatarUrl;
            }

            if(model.Gender != null)
            {
                user.Gender = model.Gender;
            }

            if(model.DateOfBirth != null)
            {
                user.DateOfBirth = model.DateOfBirth;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new UserException(UserExceptionMessages.UpdateUserFail, updatedUserId);
            }

            return new UserDTO(user);
        }

        public async Task<UserDTO> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound, userId);
            }

            return new UserDTO(user);
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotifications(ApplicationUser user)
        {
            var notifications = await _unitOfWork.Repository<Notification>().GetDbset()
                .Where(n => n.UserId == user.Id)
                .OrderByDescending(n => n.DateCreated)
                .ToListAsync();

            List<NotificationDTO> result = new List<NotificationDTO>();
            foreach(var noti in notifications)
            {
                result.Add(_mapper.Map<NotificationDTO>(noti));
            }

            return result;
        }

        public async Task<NotificationDTO> ReadNotification(long notficationId)
        {
            var noti = await _unitOfWork.Repository<Notification>().FindAsync(notficationId);
            if (noti == null)
            {
                throw new NotificationNotFoundException(UserExceptionMessages.NotificationNotFound, notficationId);
            }

            noti.IsRead = true;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<NotificationDTO>(noti);
        }

        public async Task<NotificationDTO> RemoveNotification(long notficationId)
        {
            var noti = await _unitOfWork.Repository<Notification>().FindAsync(notficationId);
            if (noti == null)
            {
                throw new NotificationNotFoundException(UserExceptionMessages.NotificationNotFound, notficationId);
            }
            noti = await _unitOfWork.Repository<Notification>().DeleteAsync(notficationId);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<NotificationDTO>(noti);
        }

        public Task TestTriggerNotification()
        {
            DailyNotificationJob job = new DailyNotificationJob(_unitOfWork, null);
            return job.DistributeNotification();
        }

        public async Task FindUserConnection(ClaimsPrincipal user, string searchingUserId)
        {
            List<UserConnectionDTO> result = new List<UserConnectionDTO>();
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!_userManager.Users.Any(u => u.Id.Equals(userId)))
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound, userId);
            }

            //var nodesBelongsToUser = await _unitOfWork.Repository<Person>().GetDbset()
            //    .Where(p => p.UserId.Equals(userId))
            //    .ToListAsync();

            //var relatedPeople = await _unitOfWork.Repository<Person>().GetDbset()
            //    .Where(p => nodesBelongsToUser.Any(n => n.FamilyTreeId == p.FamilyTreeId) 
            //        && p.UserId.Equals(searchingUserId))
            //    .ToListAsync();

            var connections = await _unitOfWork.Repository<UserConnectionModel>()
                .GetUserConnection(userId, searchingUserId)
                .ToListAsync();


        }
    }
}
