using FamilyTreeBackend.Core.Application.Helpers.ConfigModels;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Middlewares
{
    public class UserExtractionMiddleware
    {
        private readonly RequestDelegate _next;

        public UserExtractionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, UserManager<ApplicationUser> userManager)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                await extractUserFromToken(httpContext, userManager);
            }
            else
            {
                await _next(httpContext);
            }
        }

        private async Task extractUserFromToken(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            var foundUser = await userManager.GetUserAsync(context.User);

            if(foundUser == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(GenericResponseStrings.Auth_UserIsNotValid);
                await context.Response.StartAsync();
                return;
            }

            context.Items["User"] = foundUser;
            await _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class UserExtractionMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserExtractionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserExtractionMiddleware>();
        }
    }
}
