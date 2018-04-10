using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;

namespace MyMoods.Services.Injection
{
    public static class DefaultLogger
    {
        public static void Use(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .WriteTo.RollingFile(Path.Combine(env.ContentRootPath, @"logs/{Date}.log"))
               .CreateLogger();

            loggerFactory.AddSerilog(logger);
        }
    }
}
