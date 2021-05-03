using FamilyTreeBackend.Core.Application.Helpers;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Presentation.API.Controllers.Misc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class BaseExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public BaseExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext);
            } 
            catch (BaseServiceException exception)
            {
                await HandleBaseExceptionAsync(httpContext, exception);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task HandleBaseExceptionAsync(HttpContext httpContext, BaseServiceException exception)
        {
            uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
            if (statusCode != null && statusCode.HasValue)
            {
                var personResponse = new HttpResponse<BaseServiceException>(exception, exception.Message);

                await BuildResponseAsync(httpContext, (int)statusCode, JsonSerializer.Serialize(personResponse));
                return;
            }


        }

        private async Task BuildResponseAsync(HttpContext httpContext, int statusCode, string bodyResponse) 
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(bodyResponse);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BaseExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseBaseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BaseExceptionHandlerMiddleware>();
        }
    }
}
