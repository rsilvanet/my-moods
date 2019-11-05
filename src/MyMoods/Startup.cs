using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyMoods.Services.Injection;
using MyMoods.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var logger = new LoggerConfiguration()
              .MinimumLevel.Information()
              .WriteTo.RollingFile(Path.Combine(env.ContentRootPath, @"logs/{Date}.log"))
              .CreateLogger();

            loggerFactory.AddSerilog(logger);

            app.UseMiddleware<AnalyticsAuthorizationMiddleware>();
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }

        public void ConfigureJson(JsonSerializerSettings settings)
        {
            settings.Converters.Add(new StringEnumConverter());
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
        }

    }
}
