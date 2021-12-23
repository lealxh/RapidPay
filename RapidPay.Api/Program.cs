using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace RapidPay.Api
{
    public class Program
    {
        //Defining the appsettings.json as configuration source for our application
        //the DefaultConnection parameter in this file have the connection string for our application.
        public static IConfiguration Configuration { get; } =
        new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
       .Build();

        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            builder.Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseConfiguration(Configuration);//Adding the configuration defined

                }).ConfigureLogging(
                logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                });

        
    }
}
