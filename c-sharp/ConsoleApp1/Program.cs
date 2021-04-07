using JokesGenerator.Services.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JokesGenerator
{
    class Program
    {
        public static void Main(string[] args)
        {
            var host = Startup.AppStartup();
            Console.Title = AppConsts.AppName;
            var dataService = ActivatorUtilities.CreateInstance<RunnerService>(host.Services);
            dataService.Run();
            Environment.Exit(0);
        }
    }
}
