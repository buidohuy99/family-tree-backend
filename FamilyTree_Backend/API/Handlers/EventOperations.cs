using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Presentation.API.Handlers
{
    public class EventOperations
    {
        public static OperationAuthorizationRequirement Create =
        new OperationAuthorizationRequirement { Name = nameof(Create) };

        public static OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement { Name = nameof(Read) };

        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = nameof(Update) };

        public static OperationAuthorizationRequirement Reschedule =
            new OperationAuthorizationRequirement { Name = nameof(Reschedule) };

        public static OperationAuthorizationRequirement Cancel =
            new OperationAuthorizationRequirement { Name = nameof(Cancel) };

        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = nameof(Delete) };
    }
}
