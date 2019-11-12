using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using MyMoods.Hangfire.Filters;
using MyMoods.Shared.Contracts;
using MyMoods.Shared.Domain;
using System;

namespace MyMoods.Hangfire.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        private static void RegisterRecurringJobs()
        {
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

        public static void ConfigureHangfire(this IApplicationBuilder app, IConfigurationRoot config)
        {
            var dashboardPath = config.GetSection("Host").GetValue<string>("HangfirePath");

            var dashboardOptions = new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            };

            app.UseHangfireServer();
            app.UseHangfireDashboard(dashboardPath, dashboardOptions);

            GlobalJobFilters.Filters.Add(new HangfireProlongExpirationTimeAttribute());
            
            RegisterRecurringJobs();
        }
    }
}
