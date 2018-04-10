using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MyMoods.Shared.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoods.Settings
{
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

                    var company = await _storage.Companies
                        .Find(x => x.Id.Equals(oid))
                        .FirstOrDefaultAsync();

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

}
