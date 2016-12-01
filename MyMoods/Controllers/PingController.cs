using Microsoft.AspNetCore.Mvc;

namespace MyMoods.Controllers
{
    [Route("api/ping")]
    public class PingController : BaseController
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("pong");
        }
    }
}
