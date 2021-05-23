using FamilyTreeBackend.Core.Application.Helpers.Exceptions;
using FamilyTreeBackend.Core.Domain.Constants;
using FamilyTreeBackend.Presentation.API.Controllers.Misc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Middlewares
{

    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class BaseExceptionHandlerMiddleware
    {
        public class ExceptionResponse
        {
            public string Message { get; set; }
            public Dictionary<string, string> Data;
        }
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public BaseExceptionHandlerMiddleware(RequestDelegate next, ILogger<BaseExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (BaseServiceException exception)
            {
                _logger.LogInformation(exception, GenericResponseStrings.RequestProcessingError);
                await HandleBaseExceptionAsync(httpContext, exception);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        private async Task HandleBaseExceptionAsync(HttpContext httpContext, BaseServiceException exception)
        {
            var data = GetDataFromException(exception);
            uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
            var type = exception.GetType();
            if (statusCode != null && statusCode.HasValue)
            {
                var response = new ExceptionResponse()
                {
                    Message = exception.Message,
                    Data = data,
                };

                var responseStr = JsonConvert.SerializeObject(response);

                await BuildResponseAsync(httpContext, (int)statusCode, responseStr); ;
                return;
            }


        }

        private async Task BuildResponseAsync(HttpContext httpContext, int statusCode, string bodyResponse)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(bodyResponse);
        }

        private Dictionary<string, string> GetDataFromException(BaseServiceException exception)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            var properties = exception.GetType().GetProperties();

            foreach (var prop in properties)
            {
                result[prop.Name] = prop.GetValue(exception) != null ? prop.GetValue(exception).ToString() : "";

            }

            var excludedProps = typeof(Exception).GetProperties();
            foreach (var prop in excludedProps)
            {
                result.Remove(prop.Name);
            }
            return result;

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
