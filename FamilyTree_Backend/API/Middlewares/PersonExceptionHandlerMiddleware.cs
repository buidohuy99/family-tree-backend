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
    public class PersonExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public PersonExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext);
            } 
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            string genericMessage = GenericResponseStrings.AnExceptionOccuredInController;
            if (exception is BaseServiceException)
            {
                uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
                if (statusCode != null && statusCode.HasValue)
                {
                    var personResponse = new HttpResponse<string>(exception.Message, genericMessage);

                    await BuildResponseAsync(httpContext, (int)statusCode, JsonSerializer.Serialize(personResponse));
                    return;
                }
            }
                
            var response = new HttpResponse<Exception>(exception, GenericResponseStrings.InternalServerError);
            await BuildResponseAsync(httpContext, 500, JsonSerializer.Serialize(response));

        }

        private async Task BuildResponseAsync(HttpContext httpContext, int statusCode, string bodyResponse) 
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(bodyResponse);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class PersonExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UsePersonExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PersonExceptionHandlerMiddleware>();
        }
    }
}
