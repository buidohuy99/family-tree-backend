using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Handlers
{
    public class PersonOperationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Person>
    {
        private readonly IUnitOfWork _unitOfWork;
        public PersonOperationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Person resource)
        {
            var user = context.User;

            var tree = resource.FamilyTree;

            if (user?.FindFirst(ClaimTypes.NameIdentifier)?.Value.Equals(tree.Owner.Id) == true)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            //check if user is one of the editors
            foreach (var editor in tree.Editors)
            {
                if (user?.FindFirst(ClaimTypes.NameIdentifier)?.Value.Equals(editor.Id) == true)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }


            //if not none of the above, 
            //user can still update their own node if they are attached to it
            if (user?.FindFirst(ClaimTypes.NameIdentifier)?.Value.Equals(resource.UserId) == true)
            {
                if (requirement == PersonOperations.Update || requirement == PersonOperations.Read)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }

            //if all of them are not qualified
            context.Fail();
            return Task.CompletedTask;

        }

    }
}
