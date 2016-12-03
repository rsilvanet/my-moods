using Microsoft.AspNetCore.Mvc;
using MyMoods.Contracts;
using MyMoods.Domain.DTO;
using System;
using System.Threading.Tasks;

namespace MyMoods.Controllers
{
    [Route("api/forms")]
    public class FormsController : BaseController
    {
        private readonly IFormsService _formsService;
        private readonly IReviewsService _reviewsService;

        public FormsController(IFormsService formsService, IReviewsService reviewsService)
        {
            _formsService = formsService;
            _reviewsService = reviewsService;
        }

        [HttpGet("{id}/metadata")]
        public async Task<IActionResult> GetMetadata(string id)
        {
            try
            {
                return Ok(await _formsService.GetMetadataByIdAsync(id));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost("{id}/reviews")]
        public async Task<IActionResult> PostReview(string id, [FromBody]ReviewOnPostDTO dto)
        {
            try
            {
                var form = await _formsService.GetByIdAsync(id);

                if (form == null)
                {
                    return NotFound();
                }

                if (dto == null)
                {
                    return BadRequest("O conteúdo da requisição está inválido.");
                }

                var validation = await _reviewsService.ValidateToInsertAsync(form, dto);

                if (!validation.Success)
                {
                    return BadRequest(validation.Errors);
                }

                await _reviewsService.InsertAsync(validation.ParsedObject);

                return Created(validation.ParsedObject.Id.ToString());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
