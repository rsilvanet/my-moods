using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyMoods.Contracts;
using MyMoods.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MyMoods
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(x => Mongo.Database.Get());
            services.AddScoped<IStorage, Mongo.Storage>();
            services.AddScoped<IFormsService, FormsService>();
            services.AddScoped<IMoodsService, MoodsService>();
            services.AddScoped<IReviewsService, ReviewsService>();
            services.AddScoped<IUsersService, UsersService>();

            services.AddMvc();
            services.AddMvcCore().AddJsonFormatters(x => ConfigureJson(x));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
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
