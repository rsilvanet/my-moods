using Microsoft.AspNetCore.Mvc;
using MyMoods.Contracts;
using System;
using System.Threading.Tasks;

namespace MyMoods.Controllers
{
    [Route("api/forms")]
    public class FormsController : BaseController
    {
        private readonly IFormsService _formsService;

        public FormsController(IFormsService formsService)
        {
            _formsService = formsService;
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
    }
}
