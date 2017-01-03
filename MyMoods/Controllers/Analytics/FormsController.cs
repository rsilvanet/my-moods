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

        public FormsController(IFormsService formsService)
        {
            _formsService = formsService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
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

                var dto = _formsService.GetWithQuestionsAsync(form);

                return Ok(dto);
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

                var validation = await _formsService.ValidateToCreateAsync(LoggedCompanyId, dto);

                if (!validation.Success)
                {
                    return BadRequest(validation.Errors);
                }

                await _formsService.CreateAsync(validation.ParsedObject);

                return Created(validation.ParsedObject.Id.ToString());
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

                var form = await _formsService.GetByIdAsync(id);

                if (form == null)
                {
                    return NotFound();
                }

                if (form.Company.ToString() != LoggedCompanyId)
                {
                    return Forbid();
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
    }
}
