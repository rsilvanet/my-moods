﻿using Microsoft.AspNetCore.Mvc;
using MyMoods.Contracts;
using MyMoods.Domain;
using MyMoods.Domain.DTO;
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

                var form = new Form(companyValidation.ParsedObject.Id)
                {
                    Title = "Visão geral da empresa",
                    UseDefaultTags = true
                };

                await _formsService.CreateAsync(form);

                return Created(companyValidation.ParsedObject.Id.ToString());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
