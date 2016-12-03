using Microsoft.AspNetCore.Mvc;
using MyMoods.Contracts;
using MyMoods.Domain.DTO;
using System;
using System.Threading.Tasks;

namespace MyMoods.Controllers.Analytics
{
    [Route("api/analytics/reset")]
    public class ResetController : AnalyticsBaseController
    {
        private readonly IUsersService _userService;

        public ResetController(IUsersService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Reset([FromBody]ResetDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("O conteúdo da requisição está inválido.");
                }

                var user = await _userService.GetByEmailAsync(dto.Email);

                if (user == null)
                {
                    return NotFound();
                }

                await _userService.ResetPasswordAsync(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
