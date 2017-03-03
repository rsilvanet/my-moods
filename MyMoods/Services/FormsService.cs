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

        private async Task DoCommonSaveValidation(FormOnPutDTO dto, ValidationResultDTO<Form> result)
        {
            var form = result.ParsedObject;

            form.AllowMultipleReviewsAtOnce = dto.AllowMultipleReviewsAtOnce;

            if (string.IsNullOrWhiteSpace(dto.MainQuestion))
            {
                result.Error("mainQuestion", "A pergunta inicial não foi informada.");
            }
            else
            {
                form.MainQuestion = dto.MainQuestion;
            }

            if (form.Type == FormType.generalWithCustomTags || form.Type == FormType.generalOnlyCustomTags)
            {
                if (dto.CustomTags == null)
                {
                    result.Error("customTags", "Nenhuma tag foi adicionada.");
                }
                else
                {
                    var companyTags = await _tagsService.GetByCompanyAsync(form.Company.ToString(), true);
                    var selectedTags = companyTags.Where(x => dto.CustomTags.Contains(x.Id.ToString())).ToList();

                    form.CustomTags = selectedTags.Select(x => x.Id).Distinct().ToList();
                }
            }

            var questions = new List<Question>();

            if (dto.FreeText != null && dto.FreeText.Allow)
            {
                Question question = null;

                if (form.QuestionsAreLoaded)
                {
                    question = form.Questions.FirstOrDefault(x => x.Type == QuestionType.text);
                }

                if (question == null)
                {
                    question = new Question()
                    {
                        Type = QuestionType.text,
                    };
                }

                if (string.IsNullOrWhiteSpace(dto.FreeText.Title))
                {
                    result.Error("freetext.title", "O título da questão livre não foi informado.");
                }
                else
                {
                    question.Title = dto.FreeText.Title;
                }

                question.Required = dto.FreeText.Require;

                questions.Add(question);
            }

            form.LoadQuestions(questions);

            if (result.Success)
            {
                form.Validate();
            }
        }

        public async Task<Form> GetByIdAsync(string id, bool loadTags = false, bool loadQuestions = false)
        {
            var oid = new ObjectId(id);
            var form = await _storage.Forms.Find(x => x.Id.Equals(oid)).FirstOrDefaultAsync();

            if (form != null)
            {
                if (loadTags)
                {
                    var tags = await _tagsService.GetByFormAsync(form, true);

                    form.LoadTags(tags);
                }

                if (loadQuestions)
                {
                    var questions = await GetQuestionsAsync(form);

                    form.LoadQuestions(questions);
                }
            }

            return form;
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
            var form = await GetByIdAsync(id, true, true);
            var company = await _storage.Companies.Find(x => x.Id.Equals(form.Company)).FirstOrDefaultAsync();
            var moods = _moodsService.Get();

            return new FormMetadataDTO(form, company, moods);
        }

        public async Task<IList<Question>> GetQuestionsAsync(Form form)
        {
            return await _storage.Questions.Find(x => x.Form.Equals(form.Id)).ToListAsync();
        }

        public async Task InsertAsync(Form form)
        {
            if (!form.IsValidated())
            {
                throw new InvalidOperationException("Objeto inválido para gravação.");
            }

            await _storage.Forms.InsertOneAsync(form);

            #region Questions

            if (form.Questions.Any())
            {
                foreach (var question in form.Questions)
                {
                    question.Form = form.Id;
                }

                await _storage.Questions.InsertManyAsync(form.Questions);
            }

            #endregion
        }

        public async Task UpdateAsync(Form form)
        {
            if (!form.IsValidated())
            {
                throw new InvalidOperationException("Objeto inválido para gravação.");
            }

            var builder = Builders<Form>.Update
                .Set(x => x.MainQuestion, form.MainQuestion)
                .Set(x => x.CustomTags, form.CustomTags)
                .Set(x => x.AllowMultipleReviewsAtOnce, form.AllowMultipleReviewsAtOnce);

            await _storage.Forms.UpdateOneAsync(x => x.Id.Equals(form.Id), builder);

            #region Questions

            var existentQuestions = await GetQuestionsAsync(form);
            var questionsToInsert = form.Questions.Where(x => x.Id.Equals(ObjectId.Empty));
            var questionsToUpdate = form.Questions.Where(x => !x.Id.Equals(ObjectId.Empty));
            var questionsToDelete = existentQuestions.Where(e => !questionsToUpdate.Any(u => u.Id.ToString() == e.Id.ToString()));

            foreach (var question in questionsToInsert)
            {
                question.Form = form.Id;

                await _storage.Questions.InsertOneAsync(question);
            }

            foreach (var question in questionsToUpdate)
            {
                var builder2 = Builders<Question>.Update
                    .Set(x => x.Title, question.Title)
                    .Set(x => x.Required, question.Required);

                await _storage.Questions.UpdateOneAsync(x => x.Id.Equals(question.Id), builder2);
            }

            foreach (var question in questionsToDelete)
            {
                await _storage.Questions.DeleteOneAsync(x => x.Id.Equals(question.Id));
            }

            #endregion
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

            await DoCommonSaveValidation(dto, result);

            return result;
        }

        public async Task<ValidationResultDTO<Form>> ValidateToUpdateAsync(Form form, FormOnPutDTO dto)
        {
            var result = new ValidationResultDTO<Form>();
            result.ParsedObject = form;

            await DoCommonSaveValidation(dto, result);

            return result;
        }
    }
}
