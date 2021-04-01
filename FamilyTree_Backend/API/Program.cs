using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace FamilyTreeBackend.Presentation.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:3000");
                    webBuilder.ConfigureLogging((logging) => {
                        logging.ClearProviders();
                        logging.AddSerilog();
                        logging.AddConsole();
                    });
                    webBuilder.UseStartup<App>();
                });
    }
}
