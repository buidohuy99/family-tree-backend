using FamilyTreeBackend.Core.Application.DTOs;
using FamilyTreeBackend.Core.Application.Models.Auth;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Core.Application.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthResponseModel> RegisterUser(AuthRegisterModel model);
        public Task<AuthResponseModel> Login(AuthLoginModel model);
        public Task<string> RefreshAccessToken(string refreshToken);
    }
}
