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
using MyMoods.Services;
using MyMoods.Shared.Contracts;
using MyMoods.Shared.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyMoods
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            Console.WriteLine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
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

            services.AddScoped(x => Shared.Mongo.Database.Get(appStorage.Connection));
            services.AddScoped<IStorage, Shared.Mongo.Storage>();
            services.AddScoped<ICompaniesService, CompaniesService>();
            services.AddScoped<IFormsService, FormsService>();
            services.AddScoped<IMailerService, MailerService>();
            services.AddScoped<IMoodsService, MoodsService>();
            services.AddScoped<IReviewsService, ReviewsService>();
            services.AddScoped<ITagsService, TagsService>();
            services.AddScoped<IUsersService, UsersService>();
            
            services.AddSingleton(Configuration);

            var hangfireStorage = new
            {
                Connection = Configuration.GetConnectionString("HangfireConnection"),
                DatabaseName = Configuration.GetConnectionString("HangfireConnection").Split('/').Last()
            };

            services.AddHangfire(x => x.UseMongoStorage(
                hangfireStorage.Connection, 
                hangfireStorage.DatabaseName,
                new MongoStorageOptions()
                {
                    MigrationOptions = new MongoMigrationOptions()
                    {
                        Strategy = MongoMigrationStrategy.Migrate
                    }
                })
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .WriteTo.RollingFile(Path.Combine(env.ContentRootPath, @"logs/{Date}.log"))
               .CreateLogger();

            loggerFactory.AddSerilog(logger);

            ConfigureHangfire(app);
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

            RecurringJob.AddOrUpdate<IFormsService>(
                "reminder-daily", 
                x => x.EnqueueReminderAsync(NotificationRecurrence.daily), 
                "30 14 * * MON-FRI", 
                TimeZoneInfo.Utc
            );

            RecurringJob.AddOrUpdate<IFormsService>(
                "reminder-weekly", 
                x => x.EnqueueReminderAsync(NotificationRecurrence.weekly), 
                "30 14 * * WED", 
                TimeZoneInfo.Utc
            );

            RecurringJob.AddOrUpdate<IFormsService>(
                "reminder-monthly", 
                x => x.EnqueueReminderAsync(NotificationRecurrence.monthly), 
                "30 14 10 * *", 
                TimeZoneInfo.Utc
            );
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
