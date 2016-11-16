using MongoDB.Driver;
using MyMoods.Contracts;
using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoods.Services
{
    public class ReviewsService : IReviewsService
    {
        private readonly IStorage _storage;
        private readonly IMoodsService _moodsService;

        public ReviewsService(IStorage storage, IMoodsService moodsService)
        {
            _storage = storage;
            _moodsService = moodsService;
        }

        private DailySimpleDTO ResumeReviews(DateTime day, IList<Review> reviews)
        {
            var avg = reviews.Average(x => (int)x.Mood);
            var avgMood = _moodsService.GetMoodByPoints(avg);
            var topMood = reviews.GroupBy(x => x.Mood).OrderByDescending(x => x.Count()).First().Key;

            var resume = new DailySimpleDTO()
            {
                Date = day,
                Count = reviews.Count(),
                Top = new DailySimpleDTO.TopDTO()
                {
                    Mood = topMood,
                    Image = _moodsService.GetImage(topMood),
                    Count = reviews.Where(x => x.Mood == topMood).Count()
                },
                Avg = new DailySimpleDTO.AvgDTO()
                {
                    Mood = avgMood,
                    Image = _moodsService.GetImage(avgMood),
                    Points = avg
                }
            };

            return resume;
        }

        private DailyDetailedDTO DetailDailyMood(DateTime date, MoodType mood, IList<Review> reviews, IList<Question> questions, IList<Tagg> tags)
        {
            var tagsOnReviews = reviews.SelectMany(z => z.Tags).Distinct();
            var resume = ResumeReviews(date, reviews);

            var detailed = new DailyDetailedDTO()
            {
                Mood = mood,
                Image = _moodsService.GetImage(mood),
                Count = reviews.Count,
                Tags = tags.Where(x => tagsOnReviews.Contains(x.Id)).Select(x => new TagDTO(x)).ToList(),
                Questions = questions.Select(x => new QuestionWithAnswersDTO(x, ReadAnswers(x, reviews))).ToList()
            };

            return detailed;
        }

        private IList<string> ReadAnswers(Question question, IList<Review> reviews)
        {
            return reviews.SelectMany(x => x.Answers)
                .Where(x => x.Question.Equals(question.Id))
                .Select(x => x.Value)
                .ToList();
        }

        public async Task InsertAsync(Review review)
        {
            await _storage.Reviews.InsertOneAsync(review);
        }

        public async Task<ValidationResultDTO<Review>> ValidateToInsertAsync(Form form, ReviewOnPostDTO review)
        {
            var result = new ValidationResultDTO<Review>();

            result.ParsedObject.Form = form.Id;
            result.ParsedObject.Date = DateTime.UtcNow;

            #region Mood

            if (string.IsNullOrWhiteSpace(review.Mood))
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

        public async Task<IList<DailySimpleDTO>> GetResumeAsync(Form form, short timezone)
        {
            var reviews = await _storage.Reviews.Find(x => x.Form.Equals(form.Id)).ToListAsync();

            if (!reviews.Any())
            {
                return new List<DailySimpleDTO>();
            }

            var groupByDay = reviews.GroupBy(x => x.Date.AddHours(timezone).Date);

            return groupByDay.Select(x => ResumeReviews(x.Key.Date.AddHours(-timezone), x.ToList())).OrderBy(x => x.Date).ToList();
        }

        public async Task<IList<DailyDetailedDTO>> GetDailyAsync(Form form, DateTime date, short timezone)
        {
            var theDay = date.Date.AddHours(-timezone);
            var theEndOfTheDay = theDay.AddDays(1).AddMilliseconds(-1);
            var reviews = await _storage.Reviews.Find(x => x.Form.Equals(form.Id) && x.Date >= theDay && x.Date <= theEndOfTheDay).ToListAsync();

            if (!reviews.Any())
            {
                return new List<DailyDetailedDTO>();
            }

            var tags = await _storage.Tags.Find(x => true).ToListAsync();
            var questions = await _storage.Questions.Find(x => x.Form.Equals(form.Id)).ToListAsync();
            var groupByMood = reviews.GroupBy(x => x.Mood);

            return groupByMood.Select(x => DetailDailyMood(date.Date.AddHours(-timezone), x.Key, x.ToList(), questions, tags)).OrderBy(x => (int)x.Mood).ToList();
        }
    }
}
