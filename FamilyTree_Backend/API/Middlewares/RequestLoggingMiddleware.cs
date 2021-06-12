using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IRequestResponseLoggingService service)
        {
            if (apiPathFilter.IsMatch(httpContext.Request.Path.Value))
            {
                RequestResponseDataModel model = new RequestResponseDataModel();
                await LogRequest(httpContext, model);
                await LogResponse(httpContext, model);
                await service.SaveLogData(model);
            }
            else
            {
                await _next(httpContext);
            }
        }

        private async Task LogRequest(HttpContext context, RequestResponseDataModel logContainer)
        {
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
            logContainer.RequestHost = context.Request.Host.Value;
            logContainer.RequestPath = context.Request.Path;
            logContainer.RequestSchema = context.Request.Scheme;
        }

        private async Task LogResponse(HttpContext context, RequestResponseDataModel logContainer)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
            logContainer.StatusCode = context.Response.StatusCode;
            logContainer.ResponseBody = text;
            logContainer.DateCreated = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt");
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
