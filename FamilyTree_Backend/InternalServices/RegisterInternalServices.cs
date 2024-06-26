﻿using FamilyTreeBackend.Core.Application.Interfaces;
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
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IFamilyTreeService, FamilyTreeService>();
            services.AddScoped<ITreeAuthorizationService, TreeAuthorizationSerivice>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMemoryService, MemoryService>();
            services.AddScoped<IRequestResponseLoggingService, RequestResponseLoggingService>();
            #endregion

            #region Register singleton
            services.AddSingleton<IUploadService, UploadService>();
            services.AddSingleton<IEmailService, EmailService>();
            #endregion
        }
    }
}
