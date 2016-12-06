using MongoDB.Bson;
using MongoDB.Driver;
using MyMoods.Contracts;
using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace MyMoods.Services
{
    public class FormsService : IFormsService
    {
        private readonly IStorage _storage;
        private readonly IMoodsService _moodsService;

        public FormsService(IStorage storage, IMoodsService moodsService)
        {
            _storage = storage;
            _moodsService = moodsService;
        }

        public async Task<Form> GetByIdAsync(string id)
        {
            var oid = new ObjectId(id);
            var form = await _storage.Forms.Find(x => x.Id.Equals(oid)).FirstOrDefaultAsync();

            return form;
        }

        public async Task<FormWithQuestionsDTO> GetWithQuestionsAsync(Form form)
        {
            var questions = await _storage.Questions.Find(x => x.Form.Equals(form.Id)).ToListAsync();
            var dto = new FormWithQuestionsDTO(form, questions);

            return dto;
        }

        public async Task<IList<Form>> GetByCompanyAsync(string companyId)
        {
            var companyOid = new ObjectId(companyId);
            var forms = await _storage.Forms.Find(x => x.Company.Equals(companyOid)).ToListAsync();

            return forms;
        }

        public async Task<FormMetadataDTO> GetMetadataByIdAsync(string id)
        {
            var oid = new ObjectId(id);
            var form = await _storage.Forms.Find(x => x.Id.Equals(oid)).FirstOrDefaultAsync();
            var company = await _storage.Companies.Find(x => x.Id.Equals(form.Company)).FirstOrDefaultAsync();
            var questions = await _storage.Questions.Find(x => x.Form.Equals(oid)).ToListAsync();
            var tags = await _storage.Tags.Find(x => true).ToListAsync();
            var moods = _moodsService.Get();

            return new FormMetadataDTO(form, company, questions, tags, moods);
        }

        public async Task<Form> CreateFormAsync(string companyId, FormOnPostDTO dto)
        {
            var form = new Form()
            {
                Title = dto.Title,
                UseDefaultTags = dto.UseDefaultTags,
                Company = new ObjectId(companyId)
            };

            await _storage.Forms.InsertOneAsync(form);

            var question = new Question()
            {
                Type = QuestionType.text,
                Title = "Quer contar um pouco mais pra gente?",
                Required = false,
                Form = form.Id
            };

            await _storage.Questions.InsertOneAsync(question);

            return form;
        }

        public Task<ValidationResultDTO> ValidateToCreateFormAsync(FormOnPostDTO dto)
        {
            var result = new ValidationResultDTO();

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                result.Error("title", "O título não foi informado.");
            }

            return Task.FromResult(result);
        }

        public async Task UpdateFormAsync(Form form, FormOnPostDTO dto)
        {
            var builder = Builders<Form>.Update
                .Set(x => x.Title, dto.Title)
                .Set(x => x.UseDefaultTags, dto.UseDefaultTags);

            await _storage.Forms.UpdateOneAsync(x => x.Id.Equals(form.Id), builder);
        }

        public Task<ValidationResultDTO> ValidateToUpdateFormAsync(FormOnPostDTO dto)
        {
            var result = new ValidationResultDTO();

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                result.Error("title", "O título não foi informado.");
            }

            return Task.FromResult(result);
        }
    }
}
