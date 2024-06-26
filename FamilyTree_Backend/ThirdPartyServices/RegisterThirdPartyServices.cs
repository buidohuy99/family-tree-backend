﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Text;
using FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.Swagger.OperationFilters;
using FamilyTreeBackend.Core.Application.Helpers.ConfigModels;
using AutoMapper;
using FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.MapperProfiles;
using Quartz;
using FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.Quartz;
using FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.Quartz.QuartzJobs;

namespace FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices
{
    public static class RegisterThirdPartyServices
    {
        public static void RegisterServices_FromThirdParty(this IServiceCollection services, IConfiguration Configuration)
        {
            #region logging configuration
            // Set up logging
            var logFilePath = Configuration.GetSection("Serilog").GetSection("LogFilePath").Value;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
            services.AddSingleton(Log.Logger);
            #endregion

            #region Swagger configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "FamilyTree_API",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "`plz type in access token without Bearer prefix` (type in without `Bearer ` prefix)",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });
                c.OperationFilter<AuthorizationHeader_Param_OperationFilter>();
                c.EnableAnnotations();
            });

            services.AddSwaggerGenNewtonsoftSupport();
            #endregion

            #region JWT authentication configuration
            services.Configure<JWT>(Configuration.GetSection("JWT"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Get the part inside env file appsettings
                ConfigurationSection JWTInfo_Section = (ConfigurationSection)Configuration.GetSection("JWT");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = JWTInfo_Section["Issuer"],
                    ValidAudience = JWTInfo_Section["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTInfo_Section["AccessTokenKey"]))
                };
            });
            #endregion

            #region AutoMapper configuration
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PersonProfile());
                mc.AddProfile(new FamilyTreeProfile());
                mc.AddProfile(new FamilyEventProfile());
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new FamilyMemoryProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region ImageKit configuration
            services.Configure<ImageKitAccounts>(Configuration.GetSection("ImageKit"));
            #endregion

            #region JWE configuration
            services.Configure<JWEConfig>(Configuration.GetSection("JWE"));
            #endregion

            #region Email Sender configuration
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            #endregion

            #region Quartz Job configuration
            services.AddQuartz(q =>
            {
                // Use a Scoped container to create jobs. I'll touch on this later
                q.UseMicrosoftDependencyInjectionScopedJobFactory();

                //register job
                q.AddJobAndTrigger<DailyNotificationJob>(Configuration);
            });
            // Add the Quartz.NET hosted service
            services.AddQuartzHostedService(
                q => q.WaitForJobsToComplete = true);
            #endregion
        }
    }
}
