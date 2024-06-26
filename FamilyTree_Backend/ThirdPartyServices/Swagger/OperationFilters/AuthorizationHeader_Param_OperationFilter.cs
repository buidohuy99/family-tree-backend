﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.Swagger.OperationFilters
{
    public class AuthorizationHeader_Param_OperationFilter : IOperationFilter
    {
        // For each request called from Swagger, we apply this filter to create more fields in it
        // (in this case, we want to add an authorization header param in our request for routes
        // that needs to be authorized)
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var isParentControllerAuthorized = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                && !context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();

            // If there is no authorize attribute or there is allow anonymous attribute, we skip adding authorization header scheme
            if ((!context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any() && isParentControllerAuthorized) ||
                (!isParentControllerAuthorized && context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()))
            {
                // Add possible responses for these routes that needs authorization
                operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized"});

                var jwtbearerScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                };

                operation.Security = new List<OpenApiSecurityRequirement>()
                {
                    new OpenApiSecurityRequirement()
                    {
                        [jwtbearerScheme] = new string []{}
                    }
                };
            }
        }
    }
}
