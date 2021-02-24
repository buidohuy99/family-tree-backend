using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeBackend.Persistence
{
    public static class RegisterPersistenceServices
    {
        public static void RegisterServices_Persistence(this IServiceCollection services, IConfiguration Configuration)
        {
            // add db context
            // add unit of work
            // add identity
            // add authentication jwt
        }
    }
}
