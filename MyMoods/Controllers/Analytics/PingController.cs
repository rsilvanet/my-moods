using Microsoft.AspNetCore.Mvc;

namespace MyMoods.Controllers.Analytics
{
    [Route("api/analytics/ping")]
    public class PingAnalyticsController : AnalyticsBaseController
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("pong");
        }
    }
}
