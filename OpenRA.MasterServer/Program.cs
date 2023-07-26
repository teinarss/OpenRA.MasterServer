using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenRA.MasterServer.Services;

namespace OpenRA.MasterServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);

            var builder = WebApplication.CreateBuilder();
            builder.Services.AddFastEndpoints();
            builder.Services.AddHttpClient();

            builder.Services.AddTransient<ValidationService>();

            builder.Services.AddDbContext<MasterServerContext>();


            var app = builder.Build();
            app.UseFastEndpoints();

            using var scope = app.Services.CreateScope();


            using var appContext = scope.ServiceProvider.GetRequiredService<MasterServerContext>();

            appContext.Database.Migrate();


            app.Run();
            //CreateHostBuilder(args).Build().Run();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureLogging(logging =>
        //        {
        //            logging.ClearProviders();
        //            logging.AddConsole();
        //            logging.SetMinimumLevel(LogLevel.Trace);
        //        })
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }

    public class Settings
    {
        public static string DbContext { get; set; } = "Data Source=../.../../database.db";
    }
}
