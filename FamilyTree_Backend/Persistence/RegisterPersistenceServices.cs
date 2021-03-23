using FamilyTreeBackend.Core.Application.Helpers.ConfigModels;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Infrastructure.Persistence.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeBackend.Infrastructure.Persistence
{
    public static class RegisterPersistenceServices
    {
        public static void RegisterServices_Persistence(this IServiceCollection services, IConfiguration Configuration)
        {
            // add db context
            services.AddDbContext<FamilyTreeDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("FamilyTreeDbContext")));
            // add unit of work
            // add identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<FamilyTreeDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}
