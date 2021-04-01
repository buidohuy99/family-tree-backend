using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    public static class RegisterInternalServices
    {
        public static void RegisterServices_Internal(this IServiceCollection services, IConfiguration Configuration)
        {
            // add scoped services down here
            #region Persistence Layer
            services.RegisterServices_Persistence(Configuration);
            #endregion

            #region Register scoped services
            services.AddScoped<IAuthService, AuthService>();
            #endregion
        }
    }
}
