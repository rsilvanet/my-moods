using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyMoods.Hangfire.Settings;
using MyMoods.Services.Injection;
using Serilog;
using System.IO;
using System.Reflection;

namespace MyMoods
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json".ToLower(), optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            DefaultInjection.Inject(services, _configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var logger = new LoggerConfiguration()
              .MinimumLevel.Information()
              .WriteTo.RollingFile(Path.Combine(env.ContentRootPath, @"logs/{Date}.log"))
              .CreateLogger();

            loggerFactory.AddSerilog(logger);

            HangfireConfiguration.Configure(app, _configuration);
        }
    }
}
