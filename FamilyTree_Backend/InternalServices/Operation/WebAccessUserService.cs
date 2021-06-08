using AutoMapper;
using FamilyTreeBackend.Core.Application.Operation.Models;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Operation.Services
{
    public interface IWebAccessUserService
    {
        public Task<IEnumerable<WebAccessUserModel>> GetAllUser();
        public Task<WebAccessUserUpdateModel> GetUserUpdateInfo(string userId);
        public Task<WebAccessUserModel> UpdateUser(string userId, WebAccessUserUpdateModel model);
        public Task<WebAccessUserModel> GetUser(string userId);
        public Task ToggleUser(string id);
        public Task<IEnumerable<WebAccessUserModel>> FindUserByFilter(string filter);
    }

    public class WebAccessUserService : IWebAccessUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public WebAccessUserService(IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<WebAccessUserModel>> GetAllUser()
        {
            var users = await _userManager.Users.ToListAsync();

            List<WebAccessUserModel> result = new List<WebAccessUserModel>();
            foreach (var user in users)
            {
                result.Add(_mapper.Map<WebAccessUserModel>(user));
            }
            return result;
        }

        public async Task<WebAccessUserUpdateModel> GetUserUpdateInfo(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return _mapper.Map<WebAccessUserUpdateModel>(user);
        }

        public async Task<WebAccessUserModel> UpdateUser(string userId, WebAccessUserUpdateModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            _mapper.Map(model, user);
            await _userManager.UpdateAsync(user);
            return _mapper.Map<WebAccessUserModel>(user);
        }

        public async Task<WebAccessUserModel> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return _mapper.Map<WebAccessUserModel>(user);
        }

        public async Task ToggleUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.Status = !user.Status;
            await _userManager.UpdateAsync(user);
        }

        public async Task<IEnumerable<WebAccessUserModel>> FilterUser(string filter)
        {
            var users = await _userManager.Users
                .Where(u => EF.Functions.Like(u.UserName, $"%{filter}%")
                || EF.Functions.Like(u.Email, $"%{filter}%"))
                .ToListAsync();

            List<WebAccessUserModel> result = new List<WebAccessUserModel>();
            foreach (var user in users)
            {
                result.Add(_mapper.Map<WebAccessUserModel>(user));
            }
            return result;
        }

        public async Task<IEnumerable<WebAccessUserModel>> FindUserByFilter(string filter)
        {
            List<WebAccessUserModel> result = new List<WebAccessUserModel>();
            if (string.IsNullOrWhiteSpace(filter) == false)
            {
                var users = await _userManager.Users.Where(u =>
                    u.UserName.Contains(filter) || u.Email.Contains(filter))
                    .ToListAsync();

                foreach(var user in users)
                {
                    result.Add(_mapper.Map<WebAccessUserModel>(user));
                }
            }
            return result;
        }

    }
}
