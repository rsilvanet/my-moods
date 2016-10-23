using Microsoft.AspNetCore.Mvc;
using MyMoods.Contracts;
using System.Threading.Tasks;

namespace MyMoods.Controllers
{
    [Route("api/forms")]
    public class FormsController : Controller
    {
        private readonly IFormsService _formsService;

        public FormsController(IFormsService formsService)
        {
            _formsService = formsService;
        }

        [HttpGet("{id}/metadata")]
        public async Task<IActionResult> GetMetadata(string id)
        {
            return Ok(await _formsService.GetMetadataAsync(id));
        }
    }
}
