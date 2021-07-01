using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        private Regex apiPathFilter = new Regex("/api/");
        private Regex fileUploadFilter = new Regex("/file-upload/");
        private const long CONTENT_LENGTH_LIMIT = 2097152;
        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IRequestResponseLoggingService service)
        {
            if (apiPathFilter.IsMatch(httpContext.Request.Path.Value))
            {
                RequestResponseDataModel model = service.Model;
                await LogRequest(httpContext, model);
                await _next(httpContext);
                //await LogResponse(httpContext, model);
                //await service.SaveLogData(model);
            }
            else
            {
                await _next(httpContext);
            }
        }

        private async Task LogRequest(HttpContext context, RequestResponseDataModel logContainer)
        {
            logContainer.UserAgent = context.Request.Headers["User-Agent"].ToString();
            var claim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            logContainer.UserId = claim != null ? claim.Value : "";
            logContainer.RequestMethod = context.Request.Method;
            logContainer.RequestHost = context.Request.Host.Value;
            logContainer.RequestPath = context.Request.Path;
            logContainer.RequestSchema = context.Request.Scheme;

            if (context.Request.ContentLength > CONTENT_LENGTH_LIMIT)
            {
                logContainer.RequestBody = "Content length exceeded logging limit";
                return;
            }

            if ((context.Request.ContentType != null && context.Request.ContentType.Equals("multipart/form-data"))
                || fileUploadFilter.IsMatch(context.Request.Path.Value))
            {
                logContainer.RequestBody = "Content is files";
                return;
            }

            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            string requestBody;
            requestStream.Position = 0;
            using (StreamReader streamReader = new StreamReader(requestStream))
            {
                requestBody = streamReader.ReadToEnd();
            }
            context.Request.Body.Position = 0;

            logContainer.RequestBody = requestBody;


        }

        
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
