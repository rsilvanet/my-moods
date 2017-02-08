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
        public async Task<IActionResult> Get(string formId, DateTime startDate, DateTime endDate)
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

                var reviews = await _reviewsService.GetByFormAsync(form, startDate, endDate, ClientTimezone);

                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut("{id}/enable")]
        public async Task<IActionResult> Enable(string formId, string id)
        {
            try
            {
                var review = await _reviewsService.GetByIdAsync(id);

                if (review == null)
                {
                    return NotFound();
                }

                if (review.Form.ToString() != formId)
                {
                    return Forbid();
                }

                await _reviewsService.EnableAsync(review);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut("{id}/disable")]
        public async Task<IActionResult> Disable(string formId, string id)
        {
            try
            {
                var review = await _reviewsService.GetByIdAsync(id);

                if (review == null)
                {
                    return NotFound();
                }

                if (review.Form.ToString() != formId)
                {
                    return Forbid();
                }

                await _reviewsService.DisableAsync(review);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
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

                var daily = await _reviewsService.GetDayDetailedByMoodAsync(form, date, ClientTimezone);

                return Ok(daily);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet("counters/moods")]
        public async Task<IActionResult> GetCountersMoods(string formId, DateTime startDate, DateTime endDate)
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

                var counters = await _reviewsService.GetMoodsCounterAsync(form, startDate, endDate, ClientTimezone);

                return Ok(counters);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
