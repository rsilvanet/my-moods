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

        public FormsController(IFormsService formsService)
        {
            _formsService = formsService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get(bool onlyActives)
        {
            try
            {
                var forms = await _formsService.GetByCompanyAsync(LoggedCompanyId, onlyActives);

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
        public async Task<IActionResult> Post([FromBody]FormOnPostDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("O conteúdo da requisição está inválido.");
                }

                var validation = await _formsService.ValidateToInsertAsync(LoggedCompanyId, dto);

                if (!validation.Success)
                {
                    return BadRequest(validation.Errors);
                }

                await _formsService.InsertAsync(validation.ParsedObject);

                return Created(validation.ParsedObject.Id.ToString());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var form = await _formsService.GetByIdAsync(id, true, true);

                if (form == null)
                {
                    return NotFound();
                }

                if (form.Company.ToString() != LoggedCompanyId)
                {
                    return Forbid();
                }

                var dto = new FormOnGetDTO(form);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]FormOnPutDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("O conteúdo da requisição está inválido.");
                }

                var form = await _formsService.GetByIdAsync(id, true, true);

                if (form == null)
                {
                    return NotFound();
                }

                var validation = await _formsService.ValidateToUpdateAsync(form, dto);

                if (!validation.Success)
                {
                    return BadRequest(validation.Errors);
                }

                await _formsService.UpdateAsync(form);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut("{id}/enable")]
        public async Task<IActionResult> Enable(string id)
        {
            try
            {
                var form = await _formsService.GetByIdAsync(id);

                if (form == null)
                {
                    return NotFound();
                }

                if (form.Company.ToString() != LoggedCompanyId)
                {
                    return Forbid();
                }

                await _formsService.EnableAsync(form);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut("{id}/disable")]
        public async Task<IActionResult> Disable(string id)
        {
            try
            {
                var form = await _formsService.GetByIdAsync(id);

                if (form == null)
                {
                    return NotFound();
                }

                if (form.Company.ToString() != LoggedCompanyId)
                {
                    return Forbid();
                }

                await _formsService.DisableAsync(form);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
