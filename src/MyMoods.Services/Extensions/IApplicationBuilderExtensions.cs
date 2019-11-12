using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;

namespace MyMoods.Services.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void ConfigureSerilog(this IApplicationBuilder app, ILoggerFactory loggerFactory, string rootPath)
        {
            var logger = new LoggerConfiguration()
              .MinimumLevel.Information()
              .WriteTo.RollingFile(Path.Combine(rootPath, @"logs/{Date}.log"))
              .CreateLogger();

            loggerFactory.AddSerilog(logger);
        }
    }
}
