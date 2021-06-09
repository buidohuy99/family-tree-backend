using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Infrastructure.Persistence.Context;
using FamilyTreeBackend.Infrastructure.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyTreeBackend.Infrastructure.Persistence
{
    public static class RegisterOperationPersistenceServices
    {
        public static void RegisterOperationServices_Persistence(this IServiceCollection services, IConfiguration Configuration)
        {
            // add db context
            services.AddDbContext<FamilyTreeDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("FamilyTreeDbContext")));
            // add unit of work
            services.AddScoped<IUnitOfWork, GenericUnitOfWork<FamilyTreeDbContext>>();
            // add identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<FamilyTreeDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();
        }
    }
}
