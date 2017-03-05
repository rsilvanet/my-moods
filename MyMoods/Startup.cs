using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MyMoods.Contracts;
using MyMoods.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Annotations;
using System;

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
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json".ToLower(), optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(x => Mongo.Database.Get(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IStorage, Mongo.Storage>();
            services.AddScoped<ICompaniesService, CompaniesService>();
            services.AddScoped<IFormsService, FormsService>();
            services.AddScoped<IMailerService, MailerService>();
            services.AddScoped<IMoodsService, MoodsService>();
            services.AddScoped<IReviewsService, ReviewsService>();
            services.AddScoped<ITagsService, TagsService>();
            services.AddScoped<IUsersService, UsersService>();

            services.AddSingleton(Configuration);

            services.AddMvc();
            services.AddMvcCore().AddJsonFormatters(x => ConfigureJson(x));
            services.AddHangfire(x => x.UseMongoStorage(Configuration.GetConnectionString("HangfireConnection"), Configuration.GetConnectionString("HangfireConnection").Split('/').Last()));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMiddleware<AnalyticsAuthorizationMiddleware>();

            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            var dashboardPath = Configuration.GetSection("Hangfire").GetValue<string>("DashboardPath");
            var dashboardOptions = new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            };

            app.UseHangfireServer();
            app.UseHangfireDashboard(dashboardPath, dashboardOptions);
        }

        public void ConfigureJson(JsonSerializerSettings settings)
        {
            settings.Converters.Add(new StringEnumConverter());
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
        }

        public class AnalyticsAuthorizationMiddleware
        {
            private readonly IStorage _storage;
            private readonly RequestDelegate _next;

            public AnalyticsAuthorizationMiddleware(RequestDelegate next, IStorage storage)
            {
                _next = next;
                _storage = storage;
            }

            public async Task Invoke(HttpContext context)
            {
                if (context.Request.Path.Value.ToLower().StartsWith("/api/analytics/"))
                {
                    var noAuthRoutes = new string[]
                    {
                        "/api/analytics/login",
                        "/api/analytics/reset"
                    };

                    if (!noAuthRoutes.Contains(context.Request.Path.Value.ToLower()))
                    {
                        if (!context.Request.Headers.Keys.Contains("X-Company"))
                        {
                            context.Response.StatusCode = 401;
                            return;
                        }

                        ObjectId oid;
                        ObjectId.TryParse(context.Request.Headers["X-Company"], out oid);

                        var company = await _storage.Companies.Find(x => x.Id.Equals(oid)).FirstOrDefaultAsync();

                        if (company == null)
                        {
                            context.Response.StatusCode = 401;
                            return;
                        }
                    }
                }

                await _next.Invoke(context);
            }
        }

        public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                return true;
            }
        }
    }
}
