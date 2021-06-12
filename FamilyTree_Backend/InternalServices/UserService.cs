using AutoMapper;
using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.UserExceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models;
using FamilyTreeBackend.Core.Application.Models.User;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.Quartz.QuartzJobs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StopWord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
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

            var passwordResetUrl = clientSite + string.Format(ResetEmailUrl, token, email);

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

        public async Task<string> GenerateConfirmEmailUrl(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var clientSite = _configuration.GetValue<string>("ClientSite");
            var confirmEmailUrl = clientSite + string.Format(ConfirmEmailUrl, token, user.Email);
            return confirmEmailUrl;
        }

        public async Task<IdentityResult> ConfirmEmailWithToken(ConfirmEmailModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound, email: model.Email);
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);
            return result;
        }

        public IEnumerable<UserDTO> FindUser(UserFilterModel model)
        {
            var users = _userManager.Users.Where(e => e.Status == false);

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
                if (model.UsernameOrEmailContains != null)
                {
                    users = users.Where(e => e.UserName.Contains(model.UsernameOrEmailContains) || e.Email.Contains(model.UsernameOrEmailContains));
                }

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

        public FindUsersPaginationResponseModel FindUser(UserFilterModel model, PaginationModel paginationModel)
        {
            var users = _userManager.Users.Where(e => e.Status == false);

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
                if (model.UsernameOrEmailContains != null)
                {
                    users = users.Where(e => e.UserName.Contains(model.UsernameOrEmailContains) || e.Email.Contains(model.UsernameOrEmailContains));
                }

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

            users = users.Where(tr => tr.CreatedDate.CompareTo(paginationModel.CreatedBefore) <= 0);

            var totalPage = (ulong)MathF.Ceiling((ulong)users.Count() / paginationModel.ItemsPerPage);
            totalPage = totalPage <= 0 ? 1 : totalPage;

            if (paginationModel.Page > totalPage)
            {
                throw new PaginationException(GeneralExceptionMessages.PageOutOfBounds, paginationModel.Page, paginationModel.ItemsPerPage, totalPage);
            }

            users = users.Skip((int)((paginationModel.Page - 1) * paginationModel.ItemsPerPage)).Take((int)paginationModel.ItemsPerPage);

            return new FindUsersPaginationResponseModel()
            {
                Result = users.Select(e => new UserDTO(e)),
                TotalPages = totalPage,
                CurrentPage = paginationModel.Page,
                ItemsPerPage = paginationModel.ItemsPerPage
            };
        }

        public async Task<UserDTO> UpdateUser(string updatedUserId, UpdateUserModel model)
        {
            var user = await _userManager.FindByIdAsync(updatedUserId);

            if (user == null)
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound, updatedUserId);
            }

            if (model.FirstName != null)
            {
                user.FirstName = string.IsNullOrEmpty(model.FirstName) ? null : model.FirstName;
            }

            if (model.MidName != null)
            {
                user.MidName = string.IsNullOrEmpty(model.MidName) ? null : model.MidName;
            }

            if (model.LastName != null)
            {
                user.LastName = string.IsNullOrEmpty(model.LastName) ? null : model.LastName;
            }

            if (model.Address != null)
            {
                user.Address = string.IsNullOrEmpty(model.Address) ? null : model.Address;
            }

            if (model.Phone != null)
            {
                user.PhoneNumber = string.IsNullOrEmpty(model.Phone) ? null : model.Phone;
            }

            if (model.AvatarUrl != null)
            {
                user.AvatarUrl = string.IsNullOrEmpty(model.AvatarUrl) ? null : model.AvatarUrl;
            }

            if (model.Gender != null)
            {
                user.Gender = model.Gender;
            }

            if (model.DateOfBirth != null)
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
            foreach (var noti in notifications)
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

        public async Task<IEnumerable<UserConnectionDTO>> FindUserConnection(ClaimsPrincipal user, string searchingUserId)
        {
            List<UserConnectionDTO> result = new List<UserConnectionDTO>();
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!_userManager.Users.Any(u => u.Id.Equals(userId)))
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound, userId);
            }
            if (!_userManager.Users.Any(u => u.Id.Equals(searchingUserId)))
            {
                throw new UserNotFoundException(UserExceptionMessages.UserNotFound, searchingUserId);
            }

            //var nodesBelongsToUser = await _unitOfWork.Repository<Person>().GetDbset()
            //    .Where(p => p.UserId.Equals(userId))
            //    .ToListAsync();

            //var relatedPeople = await _unitOfWork.Repository<Person>().GetDbset()
            //    .Where(p => nodesBelongsToUser.Any(n => n.FamilyTreeId == p.FamilyTreeId) 
            //        && p.UserId.Equals(searchingUserId))
            //    .ToListAsync();

            var sourceUser = new SqlParameter("SourceUser", userId);
            var destinationUser = new SqlParameter("DestinationUser", searchingUserId);

            Dictionary<string, UserConnection> connections = await _unitOfWork.GetUserConnections()
                .FromSqlRaw(Sql_GetUserConnection, sourceUser, destinationUser).ToDictionaryAsync(uc => uc.SourceUserId);


            foreach (var pair in connections)
            {
                var connection = pair.Value;

                //if this is the first node in the string of connections
                if (connection.SourceUserId.Equals(userId))
                {
                    var dto = await CreateUserConnectionDTO(connection);
                    result.Add(dto);

                    //loop till it reaches the end of the connection
                    while (connection.DestinationUserId.Equals(searchingUserId) == false)
                    {
                        var nextConnection = connections.GetValueOrDefault(connection.DestinationUserId);
                        if (nextConnection == null)
                        {
                            break;
                        }
                        dto.NextConnection = await CreateUserConnectionDTO(nextConnection);
                        dto = dto.NextConnection;
                        connection = nextConnection;
                    }
                }
            }

            return result;
        }

        private async Task<UserConnectionDTO> CreateUserConnectionDTO(UserConnection connection)
        {
            var source = await _userManager.FindByIdAsync(connection.SourceUserId);
            var destination = await _userManager.FindByIdAsync(connection.SourceUserId);
            var tree = await _unitOfWork.Repository<FamilyTree>().FindAsync(connection.FamilyTreeId);
            return new UserConnectionDTO(
                _mapper.Map<UserDTO>(source),
                _mapper.Map<UserDTO>(destination),
                _mapper.Map<FamilyTreeDTO>(tree), null
                );
        }

        private const string ResetEmailUrl = "/reset-passowrd?token={0}&email={1}";
        private const string ConfirmEmailUrl = "/confirm-email?token={0}&email={1}";
        private const string Sql_GetUserConnection = @"
with
_connectedUserLevel1 as (
select p1.UserId as SourceId
, p2.UserId as DestinationId
, p1.FamilyTreeId
from Person p1 inner join Person p2 on (p1.FamilyTreeId = p2.FamilyTreeId and p2.UserId is not null)
where p1.UserId = @SourceUser
),

_connectedUserLevel2 as (
select p1.UserId as SourceId
, p2.UserId as DestinationId
, p1.FamilyTreeId
from _connectedUserLevel1 lv1 
inner join Person p1 on (lv1.SourceId = p1.UserId) 
inner join Person p2 on (p1.FamilyTreeId = p2.FamilyTreeId and p2.UserId is not null)
),

_connectedUserLevel3 as (
select p1.UserId as SourceId
, p2.UserId as DestinationId
, p1.FamilyTreeId
from _connectedUserLevel1 lv2 
inner join Person p1 on (lv2.SourceId = p1.UserId) 
inner join Person p2 on (p1.FamilyTreeId = p2.FamilyTreeId and p2.UserId is not null)
),

_map as (
select *
from _connectedUserLevel1
union
select *
from _connectedUserLevel2
union 
select *
from _connectedUserLevel3
),

_reachedDestinationConnections as (
select *
from _map
where _map.DestinationId = @DestinationUser
)


select _map.*
from _map inner join _reachedDestinationConnections 
on (_map.SourceId = _reachedDestinationConnections.SourceId 
OR _map.DestinationId = _reachedDestinationConnections.SourceId)
union
select *
from _reachedDestinationConnections";

        //        private const string Sql_GetUserConnection = @"
        //with
        //_treesConnectedToUser1 as (
        //select p.FamilyTreeId
        //from Person p
        //where p.UserId = @User1
        //),

        //_treesConnectedToUser2 as (
        //select p.FamilyTreeId
        //from Person p
        //where p.UserId = @User2
        //),

        //_connectedPeople1 as (
        //select p.UserId
        //, p.FamilyTreeId
        //from Person p
        //where
        //p.UserId is not null 
        //and p.FamilyTreeId in (select * from _treesConnectedToUser1)
        //),

        //_connectedPeople2 as (
        //select p.UserId
        //, p.FamilyTreeId
        //from Person p
        //where 
        //p.UserId is not null
        //and p.FamilyTreeId in (select * from _treesConnectedToUser2)
        //)

        //select p1.FamilyTreeId as FamilyTreeId
        //, p1.UserId as SourceUserId
        //, p2.UserId as DestinationUserId
        //from _connectedPeople1 p1 inner join _connectedPeople2 p2 on p1.FamilyTreeId = p2.FamilyTreeId";
    }
}
