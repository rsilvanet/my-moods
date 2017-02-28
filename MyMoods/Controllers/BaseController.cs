using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMoods.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult Created(string id)
        {
            return StatusCode(201, id);
        }

        public ActionResult Created(IEnumerable<string> ids)
        {
            return StatusCode(201, ids);
        }

        public ActionResult BadRequest(IDictionary<string, string> errors)
        {
            if (errors != null && errors.Any())
            {
                foreach (var erro in errors)
                {
                    ModelState.AddModelError(erro.Key, erro.Value);
                }

                return BadRequest(ModelState);
            }

            return BadRequest();
        }

        public ActionResult InternalServerError(Exception ex)
        {
            return StatusCode(500, "Não foi possível processar a requisição.");
        }
    }
}
