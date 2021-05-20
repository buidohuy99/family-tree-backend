using FamilyTreeBackend.Core.Application.Helpers.ConfigModels;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.AuthExceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Application.Models.Auth;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    public class AuthService : IAuthService
    {
        private UserManager<ApplicationUser> _userManager;
        private ILogger<AuthService> _logger;
        private JWT _jwtConfig;
        private IUnitOfWork _unitOfWork;

        public AuthService(UserManager<ApplicationUser> userManager, ILogger<AuthService> logger, IOptions<JWT> jwtConfig, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _logger = logger;
            _jwtConfig = jwtConfig.Value;
            _unitOfWork = unitOfWork;
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
                            refreshToken = await generateRefreshToken(user);
                        }

                        return new AuthResponseModel(user, accessToken, refreshToken);
                    }
                    throw new LoginUserFailException(AuthExceptionMessages.InvalidPassword);
                }
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public async Task<RefreshTokenResponseModel> RefreshAccessToken(string refreshToken)
        {
            try
            {
                //Hash the refresh token
                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_jwtConfig.RefreshTokenKey)))
                {
                    var hashed = hmac.ComputeHash(Convert.FromBase64String(refreshToken));

                    var token = _unitOfWork.GetRefreshTokens().Include(t => t.User).SingleOrDefault(e => e.Token.Equals(Convert.ToBase64String(hashed)));

                    if (token == null) // can't find token
                    {
                        throw new RefreshTokenFailException(AuthExceptionMessages.InvalidRefreshToken, refreshToken);
                    }
                    else if (token.User == null) // Bad token
                    {
                        _unitOfWork.GetRefreshTokens().Attach(token);
                        _unitOfWork.GetRefreshTokens().Remove(token);
                        await _unitOfWork.SaveChangesAsync();
                        throw new RefreshTokenFailException(AuthExceptionMessages.RefreshTokenIsCorrupted, refreshToken);
                    }
                    else
                    {
                        string accessToken = new JwtSecurityTokenHandler().WriteToken(generateAccessToken(token.User));
                        string newRefreshToken = null;
                        // purgeeeee old stuff or stuff that no longer has an owner
                        foreach (var foundToken in _unitOfWork.GetRefreshTokens().Where(t => t.UserId == null || (t.UserId.Equals(token.User.Id) && DateTime.UtcNow.CompareTo(t.ExpiredDate) > 0)))
                        {
                            _unitOfWork.GetRefreshTokens().Attach(foundToken);
                            _unitOfWork.GetRefreshTokens().Remove(foundToken);
                        }
                        //and generate a new refresh token if current one is expired
                        if (DateTime.UtcNow.CompareTo(token.ExpiredDate) > 0) 
                        {
                            newRefreshToken = await generateRefreshToken(token.User);
                        }
                        await _unitOfWork.SaveChangesAsync();
                        return new RefreshTokenResponseModel(token.User, accessToken, newRefreshToken);
                    }
                }
            } 
            catch(Exception e)
            {
                throw;
            }
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
                        refreshToken = await generateRefreshToken(newUser);
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
                throw;
            }
        }

        #region Private methods
        private JwtSecurityToken generateAccessToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                //???
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

        private async Task<string> generateRefreshToken(ApplicationUser user)
        {
            byte[] token = Guid.NewGuid().ToByteArray();

            DateTime expiredDate = DateTime.UtcNow.AddMinutes(_jwtConfig.RefreshTokenDurationInMinutes);

            //Hash true token before saving in database
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_jwtConfig.RefreshTokenKey)))
            {
                //Hashed
                var hashed = hmac.ComputeHash(token);

                //Save it in the database
                var refreshToken = new RefreshToken()
                {
                    Token = Convert.ToBase64String(hashed),
                    ExpiredDate = expiredDate,
                    UserId = user.Id
                };

                _unitOfWork.GetRefreshTokens().Add(refreshToken);
                await _unitOfWork.SaveChangesAsync();
            }

            return Convert.ToBase64String(token);
        }
        #endregion Private methods
    }
}
