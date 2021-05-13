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
        private readonly IUnitOfWork _unitOfWork;
        public TreeOperationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, FamilyTree resource)
        {
            var user = context.User;

            var entry = _unitOfWork.Entry(resource);

            if (resource.Owner == null)
            {
                entry.Reference(tr => tr.Owner).Load();
            }

            if (user?.FindFirst(ClaimTypes.NameIdentifier)?.Value.Equals(resource.Owner.Id) == true)
            {
                context.Succeed(requirement);
                return Task.CompletedTask; ;
            }
            else
            {
                if (resource.Editors == null)
                {
                    entry.Collection(tr => tr.Editors).Load();
                }

                foreach (var editor in resource.Editors)
                {
                    if (user?.FindFirst(ClaimTypes.NameIdentifier)?.Value.Equals(editor.Id) == true)
                    {
                        if (requirement.Name == TreeCRUDOperations.Update.Name || requirement.Name == TreeCRUDOperations.Create.Name)
                        {
                            context.Succeed(requirement);
                        }
                        else
                        {
                            context.Fail();
                        }
                        return Task.CompletedTask; ;

                    }
                }
            }

            return Task.CompletedTask;
        }
    }


}
