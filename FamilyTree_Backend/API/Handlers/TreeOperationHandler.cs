using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Handlers
{
    public class TreeOperationHandler : AuthorizationHandler<OperationAuthorizationRequirement, FamilyTree>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, FamilyTree resource)
        {
            var user = context.User;
            
            //check if user is owner
            if (resource.Owner == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            if (user?.FindFirst(ClaimTypes.NameIdentifier)?.Value.Equals(resource.Owner.Id) == true)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            //check if user is one of the editors
            if (resource.Editors == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            foreach (var editor in resource.Editors)
            {
                if (user?.FindFirst(ClaimTypes.NameIdentifier)?.Value.Equals(editor.Id) == true)
                {
                    if (requirement.Name == TreeCRUDOperations.Delete.Name)
                    {
                        context.Fail();
                    }
                    else
                    {
                        context.Succeed(requirement);
                    }
                    return Task.CompletedTask;

                }
            }

            return Task.CompletedTask;
        }
    }


}
