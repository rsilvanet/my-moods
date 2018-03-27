using Microsoft.AspNetCore.Mvc;
using MyMoods.Shared.Contracts;
using MyMoods.Shared.Domain.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoods.Controllers.Analytics
{
    [Route("api/analytics/tags")]
    public class TagsController : AnalyticsBaseController
    {
        private readonly ITagsService _tagsService;

        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get(bool onlyActives)
        {
            try
            {
                var forms = await _tagsService.GetByCompanyAsync(LoggedCompanyId, onlyActives);

                return Ok(forms.Select(x => new TagDTO(x)));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TagOnPostDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("O conteúdo da requisição está inválido.");
                }

                var validation = await _tagsService.ValidateToInsertAsync(LoggedCompanyId, dto);

                if (!validation.Success)
                {
                    return BadRequest(validation.Errors);
                }

                await _tagsService.InsertAsync(validation.ParsedObject);

                return Created(validation.ParsedObject.Id.ToString());
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
                var tag = await _tagsService.GetByIdAsync(id);

                if (tag == null)
                {
                    return NotFound();
                }

                if (tag.Company.ToString() != LoggedCompanyId)
                {
                    return Forbid();
                }

                await _tagsService.EnableAsync(tag);

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
                var tag = await _tagsService.GetByIdAsync(id);

                if (tag == null)
                {
                    return NotFound();
                }

                if (tag.Company.ToString() != LoggedCompanyId)
                {
                    return Forbid();
                }

                await _tagsService.DisableAsync(tag);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
