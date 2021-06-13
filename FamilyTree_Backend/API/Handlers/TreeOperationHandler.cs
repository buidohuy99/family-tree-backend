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
            
            try
            {
                //check if user is owner
                if (user?.FindFirst(ClaimTypes.NameIdentifier)?.Value.Equals(resource.OwnerId) == true)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                //check if user is one of the editors
                foreach (var editor in resource.Editors)
                {
                    if (user?.FindFirst(ClaimTypes.NameIdentifier)?.Value.Equals(editor.Id) == true)
                    {
                        if (requirement.Name == TreeOperations.Delete.Name
                            || requirement.Name == TreeOperations.AddEditor.Name
                            || requirement.Name == TreeOperations.RemoveEditor.Name)
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

                //check if user is tagged in one of the nodes
                foreach (var person in resource.People)
                {
                    if (user?.FindFirst(ClaimTypes.NameIdentifier)?.Value.Equals(person.UserId) == true)
                    {
                        if (requirement.Name == TreeOperations.Read.Name || requirement.Name == EventOperations.Read.Name
                            || requirement.Name == MemoryOperations.Read.Name)
                        {
                            context.Succeed(requirement);
                            return Task.CompletedTask;
                        }
                    }
                }
            } catch(NullReferenceException)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            
            context.Fail();
            return Task.CompletedTask;
        }
    }


}
