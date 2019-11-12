using Microsoft.AspNetCore.Mvc;
using MyMoods.Shared.Contracts;
using MyMoods.Shared.Domain.DTO;
using MyMoods.Shared.Domain.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoods.Controllers
{
    [Route("api/register")]
    public class RegisterController : BaseController
    {
        private readonly ICompaniesService _companiesService;
        private readonly IUsersService _usersService;
        private readonly IFormsService _formsService;

        public RegisterController(ICompaniesService companiesService, IUsersService usersService, IFormsService formsService)
        {
            _companiesService = companiesService;
            _usersService = usersService;
            _formsService = formsService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("O conteúdo da requisição está inválido.");
                }

                var companyValidation = await _companiesService.ValidateToInsertAsync(dto);
                var userValidation = await _usersService.ValidateToInsertAsync(dto);

                if (!companyValidation.Success || !userValidation.Success)
                {
                    return BadRequest(companyValidation.Errors.Concat(userValidation.Errors));
                }

                await _companiesService.InsertAsync(companyValidation.ParsedObject);
                await _usersService.InsertAsync(companyValidation.ParsedObject, userValidation.ParsedObject);

                var form = new FormOnPostDTO()
                {
                    Type = FormType.general,
                    Title = "Visão geral da empresa",
                    MainQuestion = "Como você está se sentindo sobre a empresa hoje?",
                    FreeText = new FreeTextDTO()
                    {
                        Allow = true,
                        Require = false,
                        Title = "Quer contar um pouco mais pra gente?"
                    }
                };

                var formValidation = await _formsService.ValidateToInsertAsync(companyValidation.ParsedObject.Id.ToString(), form);

                await _formsService.InsertAsync(formValidation.ParsedObject);

                return Created(companyValidation.ParsedObject.Id.ToString());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
