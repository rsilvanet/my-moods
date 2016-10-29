using System;
using System.Threading.Tasks;
using MyMoods.Contracts;
using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Linq;
using MongoDB.Driver;

namespace MyMoods.Services
{
    public class ReviewsService : IReviewsService
    {
        private readonly IStorage _storage;

        public ReviewsService(IStorage storage)
        {
            _storage = storage;
        }

        public async Task InsertAsync(Review review)
        {
            await _storage.Reviews.InsertOneAsync(review);
        }

        public async Task<ValidationResultDTO<Review>> ValidateToInsertAsync(Form form, ReviewOnPostDTO review)
        {
            var result = new ValidationResultDTO<Review>();

            result.ParsedObject.Form = form.Id;
            result.ParsedObject.Date = DateTime.Now;

            #region Mood

            if (string.IsNullOrEmpty(review.Mood))
            {
                result.Error("mood", "Mood não selecionado.");
            }
            else
            {
                MoodType mood;

                if (Enum.TryParse(review.Mood, out mood))
                {
                    result.ParsedObject.Mood = mood;
                }
                else
                {
                    result.Error("mood", $"Mood {review.Mood} inválido.");
                }
            }
            #endregion

            #region Tags

            if (review.Tags == null || !review.Tags.Any())
            {
                result.Error("tags", "Nenhuma tag foi selecionada.");
            }
            else
            {
                var tags = await _storage.Tags.Find(x => true).ToListAsync();

                foreach (var id in review.Tags)
                {
                    var tag = tags.FirstOrDefault(x => x.Id.ToString() == id);

                    if (tag != null)
                    {
                        result.ParsedObject.Tags.Add(tag.Id);
                    }
                    else
                    {
                        result.Error($"tags[{id}]", $"Tag inválida.");
                    }
                }
            }

            #endregion

            #region Question

            var questions = await _storage.Questions.Find(x => x.Form.Equals(form.Id)).ToListAsync();

            if (questions.Any())
            {
                foreach (var question in questions)
                {
                    var answer = review.Answers?.Where(x => question.Id.ToString() == x.Question).FirstOrDefault();

                    if (question.Required)
                    {
                        if (question.Type == QuestionType.text)
                        {
                            if (answer == null || string.IsNullOrWhiteSpace((answer.Value)))
                            {
                                result.Error($"answers[{question.Id.ToString()}]", $"Questão obrigatória não respondida.");
                            }
                        }
                        else
                        {
                            throw new NotImplementedException("Método não preparado para validar o tipo da questão.");
                        }
                    }

                    if (answer != null && !string.IsNullOrWhiteSpace((answer.Value)))
                    {
                        result.ParsedObject.Answers.Add(new Answer(question) { Value = answer.Value });
                    }
                }
            }

            #endregion

            return result;
        }
    }
}
