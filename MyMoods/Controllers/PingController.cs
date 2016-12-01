using Microsoft.AspNetCore.Mvc;
using MyMoods.Contracts;
using MyMoods.Domain;
using System.Threading.Tasks;

namespace MyMoods.Controllers
{
    [Route("api/ping")]
    public class PingController : BaseController
    {
        IMailerService _mailer;

        public PingController(IMailerService mailer)
        {
            _mailer = mailer;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _mailer.SendNewPassword(new User
            {
                Name = "Robson",
                Email = "sc.robson@gmail.com"
            });

            return Ok("pong");
        }
    }
}
