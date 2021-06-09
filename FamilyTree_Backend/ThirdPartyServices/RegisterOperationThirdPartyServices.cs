using AutoMapper;
using FamilyTreeBackend.Core.Application.Helpers.ConfigModels;
using FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices.MapperProfiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Text;

namespace FamilyTreeBackend.Infrastructure.Service.ThirdPartyServices
{
    public static class RegisterOperationThirdPartyServices
    {
        public static void RegisterOperationServices_FromThirdParty(this IServiceCollection services, IConfiguration Configuration)
        {
            #region logging configuration
            // Set up logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("SysLogs/SystemLog.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
            services.AddSingleton(Log.Logger);
            #endregion

            #region AutoMapper configuration
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new WebAccessUserProfile());
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

        }
    }
}
