using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.Mongo;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MyMoods.Contracts;
using MyMoods.Domain;
using MyMoods.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            var appStorage = new
            {
                Connection = Configuration.GetConnectionString("DefaultConnection"),
                DatabaseName = Configuration.GetConnectionString("DefaultConnection").Split('/').Last()
            };

            var hangfireStorage = new
            {
                Connection = Configuration.GetConnectionString("HangfireConnection"),
                DatabaseName = Configuration.GetConnectionString("HangfireConnection").Split('/').Last()
            };

            services.AddScoped(x => Mongo.Database.Get(appStorage.Connection));
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
            services.AddHangfire(x => x.UseMongoStorage(hangfireStorage.Connection, hangfireStorage.DatabaseName));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .WriteTo.RollingFile(Path.Combine(env.ContentRootPath, @"logs/{Date}.log"))
               .CreateLogger();

            loggerFactory.AddSerilog(logger);

            app.UseMiddleware<AnalyticsAuthorizationMiddleware>();

            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            ConfigureHangfire(app);
        }

        public void ConfigureJson(JsonSerializerSettings settings)
        {
            settings.Converters.Add(new StringEnumConverter());
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
        }

        public void ConfigureHangfire(IApplicationBuilder app)
        {
            var dashboardPath = Configuration.GetSection("Host").GetValue<string>("HangfirePath");

            var dashboardOptions = new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            };

            app.UseHangfireServer();
            app.UseHangfireDashboard(dashboardPath, dashboardOptions);

            GlobalJobFilters.Filters.Add(new ProlongExpirationTimeAttribute());

            RecurringJob.RemoveIfExists("reminder-daily");
            RecurringJob.RemoveIfExists("reminder-weekly");
            RecurringJob.RemoveIfExists("reminder-monthly");

            RecurringJob.AddOrUpdate<IFormsService>("reminder-daily", x => x.EnqueueReminderAsync(NotificationRecurrence.daily), "0 13 * * MON-FRI", TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate<IFormsService>("reminder-weekly", x => x.EnqueueReminderAsync(NotificationRecurrence.weekly), "0 13 * * WED", TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate<IFormsService>("reminder-monthly", x => x.EnqueueReminderAsync(NotificationRecurrence.monthly), "0 13 10 * *", TimeZoneInfo.Local);
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

        public class ProlongExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
        {
            public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
            {
                context.JobExpirationTimeout = TimeSpan.FromDays(90);
            }

            public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
            {
                context.JobExpirationTimeout = TimeSpan.FromDays(90);
            }
        }
    }
}
