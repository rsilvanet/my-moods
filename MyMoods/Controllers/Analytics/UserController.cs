using Microsoft.AspNetCore.Mvc;
using MyMoods.Contracts;
using MyMoods.Domain.DTO;
using System;
using System.Threading.Tasks;

namespace MyMoods.Controllers.Analytics
{
    [Route("api/analytics/users")]
    public class UserController : AnalyticsBaseController
    {
        private readonly IUsersService _userService;

        public UserController(IUsersService userService)
        {
            _userService = userService;
        }

        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword([FromBody]ChagePasswordDTO dto)
        {
            try
            {
                var user = await _userService.GetByIdAsync(LoggedUserId);

                if (user == null)
                {
                    return NotFound();
                }

                var validation = await _userService.ValidateToChangePasswordAsync(user, dto);

                if (!validation.Success)
                {
                    return BadRequest(validation.Errors);
                }

                await _userService.ChangePasswordAsync(user, dto.New);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
