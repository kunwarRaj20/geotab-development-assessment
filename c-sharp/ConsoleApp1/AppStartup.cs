using JokesGenerator.DataAccess.Jokes;
using JokesGenerator.DataAccess.Names;
using JokesGenerator.Helpers;
using JokesGenerator.Services.Jokes;
using JokesGenerator.Services.Names;
using JokesGenerator.Services.Printer;
using JokesGenerator.Services.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;

namespace JokesGenerator
{
    public static class Startup
    {
        public static IHost AppStartup()
        {
            var builder = new ConfigurationBuilder();
            ConfigSetup(builder);

            // defining Serilog configs
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            // Initiated the denpendency injection container 
            var host = Host.CreateDefaultBuilder()
                        .ConfigureServices((context, services) =>
                        {
                            services.AddTransient<IRunnerService, RunnerService>();
                            services.AddTransient<IJokesService, JokesService>();
                            services.AddTransient<INameService, NameService>();
                            services.AddTransient<IPrinterService, PrinterService>();
                            services.AddTransient<IJokesFeed, JokesFeed>();
                            services.AddTransient<INameFeed, NameFeed>();
                            services.AddTransient<AppSession>();
                            services.AddTransient<ConsolePrinter>();
                        })
                        .UseSerilog()
                        .Build();

            return host;
        }

        static void ConfigSetup(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
        }
    }
}
