using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices;
using FamilyTreeBackend.Infrastructure.Service.InternalServices;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using Microsoft.AspNetCore.Identity;
using FamilyTreeBackend.Core.Domain.Entities;
using FamilyTreeBackend.Infrastructure.Persistence.Context;
using FamilyTreeBackend.Presentation.API.Middlewares;

namespace FamilyTreeBackend.Presentation.API
{
    public class App
    {
        public App(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddOptions();

            #region Add controllers
            services.AddControllers().AddNewtonsoftJson(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            #endregion

            #region Service Layer
            services.RegisterServices_Internal(Configuration);
            #endregion

            #region Register services from Third party
            services.RegisterServices_FromThirdParty(Configuration);
            #endregion

            #region API Versioning
            // Add API Versioning to the Project
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, FamilyTreeDbContext dbContext, IServiceProvider serviceProvider)
        {
            //app.UseExceptionHandler("/error");

            app.UsePersonExceptionHandlerMiddleware();

            dbContext.Database.Migrate();

            var logger = Log.Logger;
            try
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                //Seed Default Users
                dbContext.SeedDefaultUserAsync(userManager, roleManager).Wait();
                // Seed data
                dbContext.SeedData();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred when seeding the DB.");
            }     

            app.UseCors();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region middleware Swagger
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FamilyTree API");
                //configure swagger to appear on index of API instead of /swagger
                c.RoutePrefix = string.Empty;
            });
            #endregion
        }
    }
}
