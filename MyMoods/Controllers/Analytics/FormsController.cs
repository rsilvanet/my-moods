using Microsoft.AspNetCore.Mvc;
using MyMoods.Contracts;
using MyMoods.Domain.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoods.Controllers.Analytics
{
    [Route("api/analytics/forms")]
    public class FormsController : AnalyticsBaseController
    {
        private readonly IFormsService _formsService;

        public FormsController(IFormsService formsService, IReviewsService reviewsService)
        {
            _formsService = formsService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetForms()
        {
            try
            {
                var forms = await _formsService.GetFormsByCompanyAsync(LoggedCompanyId);

                if (!forms.Any())
                {
                    return NoContent();
                }

                return Ok(forms.Select(x => new FormDTO(x)));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> PostForm([FromBody]FormOnPostDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("O conteúdo da requisição está inválido.");
                }

                if (string.IsNullOrWhiteSpace(dto.Title))
                {
                    return BadRequest("O título não foi informado.");
                }

                var form = await _formsService.GenerateDefaultForm(LoggedCompanyId, dto.Title);

                return Created(form.Id.ToString());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutForm(string id, [FromBody]FormOnPostDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("O conteúdo da requisição está inválido.");
                }

                if (string.IsNullOrWhiteSpace(dto.Title))
                {
                    return BadRequest("O título não foi informado.");
                }

                var form = await _formsService.GetFormAsync(id);

                if (form == null)
                {
                    return NotFound();
                }

                await _formsService.RenameFormAsync(form, dto.Title);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
