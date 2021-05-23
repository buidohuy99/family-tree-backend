using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Handlers
{
    public static class TreeOperations
    {
        public static OperationAuthorizationRequirement Create =
        new OperationAuthorizationRequirement { Name = nameof(Create) };

        public static OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement { Name = nameof(Read) };

        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = nameof(Update) };

        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = nameof(Delete) };

        public static OperationAuthorizationRequirement AddEditor =
            new OperationAuthorizationRequirement { Name = nameof(AddEditor) };

        public static OperationAuthorizationRequirement RemoveEditor =
            new OperationAuthorizationRequirement { Name = nameof(RemoveEditor) };
    }
}
