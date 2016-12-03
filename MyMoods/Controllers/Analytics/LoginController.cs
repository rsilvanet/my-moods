using Microsoft.AspNetCore.Mvc;
using MyMoods.Contracts;
using MyMoods.Domain.DTO;
using System;
using System.Threading.Tasks;

namespace MyMoods.Controllers.Analytics
{
    [Route("api/analytics/login")]
    public class LoginController : AnalyticsBaseController
    {
        private readonly IUsersService _userService;

        public LoginController(IUsersService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDTO dto)
        {
            try
            {
                var user = await _userService.AuthenticateAsync(dto.Email, dto.Password);

                if (user == null)
                {
                    return Unauthorized();
                }

                var userDTO = new UserDTO(user);

                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
