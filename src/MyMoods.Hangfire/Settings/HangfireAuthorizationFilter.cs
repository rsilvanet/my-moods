using Hangfire.Dashboard;

namespace MyMoods.Hangfire.Settings
{

    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
