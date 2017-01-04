using MongoDB.Bson;
using MongoDB.Driver;
using MyMoods.Contracts;
using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace MyMoods.Services
{
    public class FormsService : IFormsService
    {
        private readonly IStorage _storage;
        private readonly IMoodsService _moodsService;
        private readonly ITagsService _tagsService;

        public FormsService(IStorage storage, IMoodsService moodsService, ITagsService tagsService)
        {
            _storage = storage;
            _moodsService = moodsService;
            _tagsService = tagsService;
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

        public async Task<IList<Form>> GetByCompanyAsync(string companyId, bool onlyActives)
        {
            var companyOid = new ObjectId(companyId);
            var forms = await _storage.Forms.Find(x => x.Company.Equals(companyOid)).ToListAsync();

            if (onlyActives)
            {
                forms = forms.Where(x => x.Active).ToList();
            }

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

        public async Task InsertAsync(Form form)
        {
            if (!form.IsValidated())
            {
                throw new InvalidOperationException("Objeto inválido para gravação.");
            }

            await _storage.Forms.InsertOneAsync(form);

            foreach (var question in form.Questions)
            {
                question.Form = form.Id;
            }

            await _storage.Questions.InsertManyAsync(form.Questions);
        }

        public async Task UpdateAsync(Form form)
        {
            if (!form.IsValidated())
            {
                throw new InvalidOperationException("Objeto inválido para gravação.");
            }

            var builder = Builders<Form>.Update.Set(x => x.Title, form.Title);

            await _storage.Forms.UpdateOneAsync(x => x.Id.Equals(form.Id), builder);
        }

        public async Task EnableAsync(Form form)
        {
            var builder = Builders<Form>.Update.Set(x => x.Active, true);

            await _storage.Forms.UpdateOneAsync(x => x.Id.Equals(form.Id), builder);
        }

        public async Task DisableAsync(Form form)
        {
            var builder = Builders<Form>.Update.Set(x => x.Active, false);

            await _storage.Forms.UpdateOneAsync(x => x.Id.Equals(form.Id), builder);
        }

        public async Task<ValidationResultDTO<Form>> ValidateToInsertAsync(string companyId, FormOnPostDTO dto)
        {
            var result = new ValidationResultDTO<Form>();

            result.ParsedObject.Active = true;
            result.ParsedObject.Company = new ObjectId(companyId);

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                result.Error("title", "O título não foi informado.");
            }
            else
            {
                result.ParsedObject.Title = dto.Title;
            }

            if (!dto.Type.HasValue)
            {
                result.Error("type", "Tipo não selecionado ou inválido.");
            }
            else
            {
                result.ParsedObject.Type = dto.Type.Value;
            }

            if (string.IsNullOrWhiteSpace(dto.MainQuestion))
            {
                result.Error("mainQuestion", "A pergunta inicial não foi informada.");
            }
            else
            {
                result.ParsedObject.MainQuestion = dto.MainQuestion;
            }

            if (dto.Type == FormType.generalWithCustomTags || dto.Type == FormType.generalOnlyCustomTags)
            {
                if (dto.CustomTags == null)
                {
                    result.Error("customTags", "Nenhuma tag foi adicionada.");
                }
                else
                {
                    var companyTags = await _tagsService.GetByCompanyAsync(companyId, true);
                    var selectedTags = companyTags.Where(x => dto.CustomTags.Contains(x.Id.ToString())).ToList();

                    result.ParsedObject.CustomTags = selectedTags.Select(x => x.Id).Distinct().ToList();
                }
            }

            var questions = new List<Question>();

            if (dto.FreeText != null && dto.FreeText.Allow)
            {
                var question = new Question()
                {
                    Type = QuestionType.text,
                    Required = dto.FreeText.Require,
                };

                if (string.IsNullOrWhiteSpace(dto.FreeText.Title))
                {
                    result.Error("freetext.title", "O título da questão livre não foi informado.");
                }
                else
                {
                    question.Title = dto.FreeText.Title;
                }

                questions.Add(question);
            }

            result.ParsedObject.LoadQuestions(questions);

            if (result.Success)
            {
                result.ParsedObject.Validate();
            }

            return result;
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
