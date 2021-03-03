using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    public static class RegisterInternalServices
    {
        public static void RegisterServices_Internal(this IServiceCollection services, IConfiguration Configuration)
        {
            // add scoped services down here
        }
    }
}
