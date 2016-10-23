using Microsoft.AspNetCore.Mvc;
using MyMoods.Contracts;

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
        public IActionResult GetMetadata(string id)
        {
            return Ok(_formsService.GetMetadata(id));
        }
    }
}
