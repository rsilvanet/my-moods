using Microsoft.AspNetCore.Mvc;
using MyMoods.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoods.Controllers.Analytics
{
    [Route("api/analytics/forms/{formId}/reviews")]
    public class ReviewsController : AnalyticsBaseController
    {
        private readonly IFormsService _formsService;
        private readonly IReviewsService _reviewsService;

        public ReviewsController(IFormsService formsService, IReviewsService reviewsService)
        {
            _formsService = formsService;
            _reviewsService = reviewsService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get(string formId, DateTime date)
        {
            var form = await _formsService.GetByIdAsync(formId);

            if (form == null)
            {
                return NotFound();
            }

            if (form.Company.ToString() != LoggedCompanyId)
            {
                return Forbid();
            }

            var reviews = await _reviewsService.GetByForm(form, date, ClientTimezone);

            if (!reviews.Any())
            {
                return NoContent();
            }

            return Ok(reviews);
        }

        [HttpGet("resume")]
        public async Task<IActionResult> GetResume(string formId)
        {
            try
            {
                var form = await _formsService.GetByIdAsync(formId);

                if (form == null)
                {
                    return NotFound();
                }

                if (form.Company.ToString() != LoggedCompanyId)
                {
                    return Forbid();
                }

                var resume = await _reviewsService.GetResumeAsync(form, ClientTimezone);

                if (resume == null || !resume.Any())
                {
                    return NoContent();
                }

                return Ok(resume);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDaily(string formId, DateTime date)
        {
            try
            {
                var form = await _formsService.GetByIdAsync(formId);

                if (form == null)
                {
                    return NotFound();
                }

                if (form.Company.ToString() != LoggedCompanyId)
                {
                    return Forbid();
                }

                var daily = await _reviewsService.GetDailyAsync(form, date, ClientTimezone);

                if (daily == null || !daily.Any())
                {
                    return NoContent();
                }

                return Ok(daily);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
