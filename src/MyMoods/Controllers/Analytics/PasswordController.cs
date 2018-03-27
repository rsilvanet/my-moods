using Microsoft.AspNetCore.Mvc;
using MyMoods.Shared.Contracts;
using MyMoods.Shared.Domain.DTO;
using System;
using System.Threading.Tasks;

namespace MyMoods.Controllers.Analytics
{
    [Route("api/analytics/password")]
    public class PasswordController : AnalyticsBaseController
    {
        private readonly IUsersService _userService;

        public PasswordController(IUsersService userService)
        {
            _userService = userService;
        }

        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody]ChagePasswordDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("O conteúdo da requisição está inválido.");
                }

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
