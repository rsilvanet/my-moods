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

        public async Task CreateAsync(Form form)
        {
            if (!form.ValidateWasCalled())
            {
                throw new InvalidOperationException("O objeto não foi previamente validado.");
            }

            var question = new Question()
            {
                Type = QuestionType.text,
                Title = "Quer contar um pouco mais pra gente?",
                Required = false,
                Form = form.Id
            };

            await _storage.Forms.InsertOneAsync(form);
            await _storage.Questions.InsertOneAsync(question);
        }

        public Task<ValidationResultDTO<Form>> ValidateToCreateAsync(string companyId, FormOnPostDTO dto)
        {
            var result = new ValidationResultDTO<Form>();

            result.ParsedObject.Company = new ObjectId(companyId);
            result.ParsedObject.UseDefaultTags = dto.UseDefaultTags;

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                result.Error("title", "O título não foi informado.");
            }
            else
            {
                result.ParsedObject.Title = dto.Title;
            }

            result.ParsedObject.Validate();

            return Task.FromResult(result);
        }

        public async Task UpdateAsync(Form form)
        {
            if (!form.ValidateWasCalled())
            {
                throw new InvalidOperationException("O objeto não foi previamente validado.");
            }

            var builder = Builders<Form>.Update
                .Set(x => x.Title, form.Title)
                .Set(x => x.UseDefaultTags, form.UseDefaultTags);

            await _storage.Forms.UpdateOneAsync(x => x.Id.Equals(form.Id), builder);
        }

        public Task<ValidationResultDTO<Form>> ValidateToUpdateAsync(Form form, FormOnPutDTO dto)
        {
            var result = new ValidationResultDTO<Form>();
            result.ParsedObject = form;

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                result.Error("title", "O título não foi informado.");
            }
            else
            {
                form.Title = dto.Title;
            }

            form.Validate();

            return Task.FromResult(result);
        }
    }
}
