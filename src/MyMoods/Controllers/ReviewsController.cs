using Microsoft.AspNetCore.Mvc;
using MyMoods.Shared.Contracts;
using MyMoods.Shared.Domain;
using MyMoods.Shared.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> PostOne(string formId, [FromBody]ReviewOnPostDTO dto)
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

        [HttpPost("many")]
        public async Task<IActionResult> PostMany(string formId, [FromBody]IList<ReviewOnPostDTO> dtoList)
        {
            try
            {
                if (dtoList == null)
                {
                    return BadRequest("O conteúdo da requisição está inválido.");
                }

                var form = await _formsService.GetByIdAsync(formId);

                if (form == null)
                {
                    return NotFound();
                }

                var reviews = new List<Review>();

                foreach (var dto in dtoList)
                {
                    var validation = await _reviewsService.ValidateToInsertAsync(form, dto);

                    if (!validation.Success)
                    {
                        return BadRequest(validation.Errors);
                    }

                    reviews.Add(validation.ParsedObject);
                }

                await _reviewsService.InsertManyAsync(reviews);

                return Created(reviews.Select(x => x.Id.ToString()));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}
