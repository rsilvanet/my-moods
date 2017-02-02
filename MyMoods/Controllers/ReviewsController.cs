using Microsoft.AspNetCore.Mvc;
using MyMoods.Contracts;
using MyMoods.Domain.DTO;
using System;
using System.Threading.Tasks;

namespace MyMoods.Controllers
{
    [Route("api/forms/{formId}/reviews")]
    public class ReviewsController : BaseController
    {
        private readonly IFormsService _formsService;
        private readonly IReviewsService _reviewsService;

        public ReviewsController(IFormsService formsService, IReviewsService reviewsService)
        {
            _formsService = formsService;
            _reviewsService = reviewsService;
        }

        [HttpPost("")]
        public async Task<IActionResult> PostReview(string formId, [FromBody]ReviewOnPostDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("O conteúdo da requisição está inválido.");
                }

                var form = await _formsService.GetByIdAsync(formId);

                if (form == null)
                {
                    return NotFound();
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
