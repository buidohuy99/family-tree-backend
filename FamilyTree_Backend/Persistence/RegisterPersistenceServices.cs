using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Persistence.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            services.AddDbContext<FamilyTreeDbContext>(optionsBuilder =>
            {
                // Configure connection to MySQL
                ConfigurationSection dbInfo_Section = (ConfigurationSection)Configuration.GetSection("DbInfo");
                string connectionString = string.Format(@"server={0};port={1};user={2};password={3};database={4}",
                     dbInfo_Section["Host"], dbInfo_Section["Port"], dbInfo_Section["Username"],
                     dbInfo_Section["Password"], dbInfo_Section["DbName"]);
                optionsBuilder.UseMySQL(connectionString, b => b.MigrationsAssembly(typeof(FamilyTreeDbContext).Assembly.FullName));
            }, ServiceLifetime.Scoped);
            // add unit of work
            // add identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<FamilyTreeDbContext>()
                .AddDefaultTokenProviders();
            // add authentication jwt
        }
    }
}
