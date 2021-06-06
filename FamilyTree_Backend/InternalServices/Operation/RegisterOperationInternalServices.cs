﻿using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Infrastructure.Persistence;
using FamilyTreeBackend.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Operation.Services;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    public static class RegisterOperationInternalServices
    {
        public static void RegisterOperationServices_Internal(this IServiceCollection services, IConfiguration Configuration)
        {
            // add scoped services down here
            #region Persistence Layer
            services.RegisterServices_Persistence(Configuration);
            #endregion

            #region Register scoped services
            services.AddScoped<IWebAccessUserService, WebAccessUserService>();
            #endregion

            #region Register singleton
            services.AddSingleton<IUploadService, UploadService>();
            services.AddSingleton<IEmailService, EmailService>();
            #endregion
        }
    }
}
