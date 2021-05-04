using FamilyTreeBackend.Core.Application.Helpers.ConfigModels;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.AuthExceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.Auth;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    public class AuthService : IAuthService
    {
        private UserManager<ApplicationUser> _userManager;
        private ILogger<AuthService> _logger;
        private JWT _jwtConfig;

        public AuthService(UserManager<ApplicationUser> userManager, ILogger<AuthService> logger, IOptions<JWT> jwtConfig)
        {
            _userManager = userManager;
            _logger = logger;
            _jwtConfig = jwtConfig.Value;

        }

        private JwtSecurityToken generateAccessToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim("uid", user.Id)
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.AccessTokenKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenDurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        private JwtSecurityToken generateRefreshToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim("uid", user.Id)
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.RefreshTokenKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfig.RefreshTokenDurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<AuthResponseModel> Login(AuthLoginModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.UsernameOrEmail) ?? await _userManager.FindByEmailAsync(model.UsernameOrEmail);
            
                if(user == null)
                {
                    throw new UserNotFoundException(UserExceptionMessages.UserNotFound);
                }
                else
                {
                    var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (checkPassword)
                    {
                        string accessToken = new JwtSecurityTokenHandler().WriteToken(generateAccessToken(user));

                        string refreshToken = null;
                        if (model.GetRefreshToken)
                        {
                            refreshToken = new JwtSecurityTokenHandler().WriteToken(generateRefreshToken(user));
                        }

                        return new AuthResponseModel(user, accessToken, refreshToken);
                    }
                    throw new LoginUserFailException(AuthExceptionMessages.InvalidPassword);
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, LoggingMessages.AuthService_ErrorMessage);
                throw;
            }
        }

        public async Task<string> RefreshAccessToken(string refreshToken)
        {
            // will implement this later
            throw new NotImplementedException();
        }

        public async Task<AuthResponseModel> RegisterUser(AuthRegisterModel model)
        {
            try
            {
                var sameUsernameUser = await _userManager.FindByNameAsync(model.UserName);

                if (sameUsernameUser != null)
                {
                    throw new RegisterUserFailException(AuthExceptionMessages.UsernameAlreadyExists, username: model.UserName);
                }

                var sameEmailUser = await _userManager.FindByEmailAsync(model.Email);

                if (sameEmailUser != null)
                {
                    throw new RegisterUserFailException(AuthExceptionMessages.EmailAlreadyExists, email: model.Email);
                }

                var newUser = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MidName = model.MidName,
                };
                var identityResult = await _userManager.CreateAsync(newUser, model.Password);

                if (identityResult.Succeeded)
                {
                    string accessToken = new JwtSecurityTokenHandler().WriteToken(generateAccessToken(newUser));

                    string refreshToken = null;
                    if (model.GetRefreshToken)
                    {
                        refreshToken = new JwtSecurityTokenHandler().WriteToken(generateRefreshToken(newUser));
                    }

                    return new AuthResponseModel(newUser, accessToken, refreshToken);
                }
                else
                {
                    throw new RegisterUserFailException(AuthExceptionMessages.RegisterUserFail, identityResult.Errors.ToList());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LoggingMessages.AuthService_ErrorMessage);
                throw;
            }
        }
    }
}
