﻿using Microsoft.AspNetCore.Mvc;
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
                var forms = await _formsService.GetByCompanyAsync(LoggedCompanyId);

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

                var validation = await _formsService.ValidateToCreateFormAsync(dto);

                if (!validation.Success)
                {
                    return BadRequest(validation.Errors);
                }

                var form = await _formsService.CreateFormAsync(LoggedCompanyId, dto);

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

                var validation = await _formsService.ValidateToUpdateFormAsync(dto);

                if (!validation.Success)
                {
                    return BadRequest(validation.Errors);
                }

                var form = await _formsService.GetByIdAsync(id);

                if (form == null)
                {
                    return NotFound();
                }

                await _formsService.UpdateFormAsync(form, dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
