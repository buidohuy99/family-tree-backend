using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CheckClaimMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckClaimMiddleware(RequestDelegate next, UserManager<ApplicationUser> userManager)
        {
            _next = next;
            _userManager = userManager;
        }

        public Task Invoke(HttpContext httpContext)
        {

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CheckClaimMiddlewareExtensions
    {
        public static IApplicationBuilder UseCheckClaimMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckClaimMiddleware>();
        }
    }
}
