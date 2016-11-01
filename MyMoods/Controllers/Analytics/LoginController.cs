using Microsoft.AspNetCore.Mvc;
using MyMoods.Contracts;
using MyMoods.Domain.DTO;
using System;
using System.Threading.Tasks;

namespace MyMoods.Controllers.Analytics
{
    [Route("api/analytics/login")]
    public class LoginController : BaseController
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDTO login)
        {
            try
            {
                var user = await _userService.AuthenticateAsync(login.Email, login.Password);

                if (user == null)
                {
                    return Unauthorized();
                }

                var dto = new UserDTO(user);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
