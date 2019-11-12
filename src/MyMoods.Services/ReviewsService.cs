using MongoDB.Bson;
using MongoDB.Driver;
using MyMoods.Shared.Contracts;
using MyMoods.Shared.Domain;
using MyMoods.Shared.Domain.DTO;
using MyMoods.Shared.Extensions;
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
        private readonly ITagsService _tagsService;
        private readonly IFormsService _formsService;

        public ReviewsService(IStorage storage, IMoodsService moodsService, ITagsService tagsService, IFormsService formsService)
        {
            _storage = storage;
            _moodsService = moodsService;
            _tagsService = tagsService;
            _formsService = formsService;
        }

        private ReviewsResumeDTO ResumeReviews(DateTime day, IList<Review> reviews)
        {
            var avg = reviews.Average(x => _moodsService.Evaluate(x.Mood));
            var avgMood = _moodsService.GetFromPoints(avg);
            var topMood = reviews.GroupBy(x => x.Mood).OrderByDescending(x => x.Count()).First().Key;

            var resume = new ReviewsResumeDTO()
            {
                Date = day,
                Count = reviews.Count(),
                Top = new ReviewsResumeDTO.TopDTO()
                {
                    Mood = topMood,
                    Count = reviews.Where(x => x.Mood == topMood).Count()
                },
                Avg = new ReviewsResumeDTO.AvgDTO()
                {
                    Mood = avgMood,
                    Points = avg
                }
            };

            return resume;
        }

        private ReviewsDetailedByMoodDTO DetailDailyMood(DateTime date, MoodType mood, IList<Review> reviews, IList<Question> questions, IList<Tagg> tags)
        {
            reviews = reviews.Where(x => x.Mood == mood).ToList();

            var resume = ResumeReviews(date, reviews);
            var tagsCounters = new List<TagCounterDTO>();
            var tagsOnReviews = reviews.SelectMany(z => z.Tags).GroupBy(x => x);

            foreach (var item in tagsOnReviews)
            {
                var tag = tags.FirstOrDefault(x => x.Id.ToString() == item.Key.ToString());

                if (tag != null)
                {
                    tagsCounters.Add(new TagCounterDTO(tag, item.Count()));
                }
            }

            var detailed = new ReviewsDetailedByMoodDTO()
            {
                Mood = mood,
                Count = reviews.Count,
                Tags = tagsCounters.OrderByDescending(x => x.Count).ToList(),
                Questions = questions.Select(x => new QuestionWithAnswersDTO(x, ReadAnswers(x, reviews, mood))).ToList()
            };

            return detailed;
        }

        private IList<string> ReadAnswers(Question question, IList<Review> reviews, MoodType? mood = null)
        {
            if (mood.HasValue)
            {
                reviews = reviews.Where(x => x.Mood == mood).ToList();
            }

            return reviews.SelectMany(x => x.Answers)
                .Where(x => x.Question.Equals(question.Id))
                .Where(x => !string.IsNullOrWhiteSpace(x.Value))
                .Select(x => x.Value)
                .ToList();
        }

        private async Task<IList<Review>> GetWithBasicFiltersAsync(Form form, DateTime? startDate, DateTime? endDate, bool onlyActives, short timezone)
        {
            var reviews = await _storage.Reviews.Find(x => x.Form.Equals(form.Id)).ToListAsync();

            if (startDate.HasValue)
            {
                reviews = reviews.Where(x => x.Date >= startDate.Value.Date.AddHours(-timezone)).ToList();
            }

            if (endDate.HasValue)
            {
                reviews = reviews.Where(x => x.Date < endDate.Value.Date.AddHours(-timezone).AddDays(1)).ToList();
            }

            if (onlyActives)
            {
                reviews = reviews.Where(x => x.Active).ToList();
            }

            return reviews;
        }

        private TagType GetTagType(IList<Tagg> tags, ObjectId tag)
        {
            return tags.FirstOrDefault(x => x.Id.ToString() == tag.ToString())?.Type ?? TagType.undefined;
        }

        public async Task<Review> GetByIdAsync(string id)
        {
            var oid = new ObjectId(id);
            var review = await _storage.Reviews.Find(x => x.Id.Equals(oid)).FirstOrDefaultAsync();

            return review;
        }

        public async Task<IList<ReviewDTO>> GetByFormAsync(Form form, DateTime startDate, DateTime endDate, short timezone)
        {
            var tags = await _tagsService.GetByFormAsync(form, false);
            var reviews = await GetWithBasicFiltersAsync(form, startDate, endDate, false, timezone);

            return reviews.Select(x => new ReviewDTO(x, tags)).ToList();
        }

        public async Task<IList<ReviewsResumeDTO>> GetResumeAsync(Form form, short timezone)
        {
            var reviews = await GetWithBasicFiltersAsync(form, null, null, true, timezone);

            if (!reviews.Any())
            {
                return new List<ReviewsResumeDTO>();
            }

            var groupByDay = reviews.GroupBy(x => x.Date.AddHours(timezone).Date);

            return groupByDay.Select(x => ResumeReviews(x.Key.Date.AddHours(-timezone), x.ToList())).OrderBy(x => x.Date).ToList();
        }

        public async Task<IList<ReviewsDetailedByMoodDTO>> GetDayDetailedByMoodAsync(Form form, DateTime date, short timezone)
        {
            var reviews = await GetWithBasicFiltersAsync(form, date, date, true, timezone);

            if (!reviews.Any())
            {
                return new List<ReviewsDetailedByMoodDTO>();
            }

            var tags = await _tagsService.GetByFormAsync(form, false);
            var questions = await _formsService.GetQuestionsAsync(form);
            var groupByMood = reviews.GroupBy(x => x.Mood);

            return groupByMood.Select(x => DetailDailyMood(date.Date.AddHours(-timezone), x.Key, x.ToList(), questions, tags)).OrderBy(x => _moodsService.Evaluate(x.Mood)).ToList();
        }

        public async Task<IList<MoodCounterDTO>> GetMoodsCounterAsync(Form form, DateTime startDate, DateTime endDate, short timezone)
        {
            var counts = new List<MoodCounterDTO>();
            var reviews = await GetWithBasicFiltersAsync(form, startDate, endDate, true, timezone);
            var group = reviews.GroupBy(x => x.Mood);

            return group.Select(x => new MoodCounterDTO(x.Key, x.Count())).ToList();
        }

        public async Task<IList<TagCounterWithMoodsDTO>> GetTagsCounterAsync(Form form, DateTime startDate, DateTime endDate, short timezone)
        {
            var counts = new List<TagCounterWithMoodsDTO>();
            var tags = await _tagsService.GetByFormAsync(form, false);
            var reviews = await GetWithBasicFiltersAsync(form, startDate, endDate, true, timezone);
            var dictionary = new Dictionary<string, ICollection<MoodCounterDTO>>();

            foreach (var reviewsByMood in reviews.GroupBy(x => x.Mood))
            {
                var mood = reviewsByMood.Key;

                var tagsFromReviews = reviewsByMood
                    .Where(x => x.Tags != null)
                    .SelectMany(x => x.Tags)
                    .GroupBy(x => x.ToString());

                foreach (var tag in tagsFromReviews)
                {
                    dictionary.AddOrAppend(tag.Key, new MoodCounterDTO(mood, tag.Count()));
                }
            }

            foreach (var item in dictionary)
            {
                var tag = tags.First(z => z.Id.ToString() == item.Key);
                var count = item.Value.Select(x => x.Count).Sum();

                counts.Add(new TagCounterWithMoodsDTO(tag, count, item.Value.OrderBy(x => x.Mood).ToList()));
            }

            return counts;
        }

        public async Task<IList<MaslowCounterDTO>> GetMaslowCounterAsync(Form form, DateTime startDate, DateTime endDate, short timezone)
        {
            var tags = await _tagsService.GetByFormAsync(form, false);
            var reviews = await GetWithBasicFiltersAsync(form, startDate, endDate, true, timezone);
            var dictionary = new Dictionary<string, ICollection<double>>();

            foreach (var reviewsByMood in reviews.GroupBy(x => x.Mood))
            {
                var mood = reviewsByMood.Key;

                var tagsFromReviews = reviewsByMood
                    .Where(x => x.Tags != null)
                    .SelectMany(x => x.Tags)
                    .ToList();

                foreach (var tag in tagsFromReviews)
                {
                    dictionary.AddOrAppend(tag.ToString(), _moodsService.Evaluate(mood));
                }
            }

            var newDictionary = dictionary.ToDictionary(
                x => tags.First(z => z.Id.ToString() == x.Key),
                x => x.Value);

            var counters = newDictionary.GroupBy(x => x.Key.Type)
                .Select(x => new
                {
                    Key = x.Key,
                    Items = x.SelectMany(z => z.Value)
                })
                .Select(x => new MaslowCounterDTO(x.Key, x.Items.Count(), x.Items.Sum()))
                .ToList();

            foreach (var area in Enum.GetValues(typeof(TagType)).OfType<TagType>().Where(x => x != TagType.undefined))
            {
                if (!counters.Any(x => x.Area == area))
                {
                    counters.Add(new MaslowCounterDTO(area, 0, 0));
                }
            }

            return counters.OrderBy(x => (int)x.Area).ToList();
        }

        public async Task<IList<AnswerByMoodDTO>> GetAnswersByMoodAsync(Form form, DateTime startDate, DateTime endDate, short timezone)
        {
            var reviews = await GetWithBasicFiltersAsync(form, startDate, endDate, true, timezone);
            var questions = await _formsService.GetQuestionsAsync(form);

            var items = new List<AnswerByMoodDTO>();

            foreach (var group in reviews.GroupBy(x => x.Mood))
            {
                var mood = group.Key;
                var reviewsByMood = group.ToList();
                var questionsWithAnswers = new List<QuestionWithAnswersDTO>();

                foreach (var question in questions)
                {
                    questionsWithAnswers.Add(new QuestionWithAnswersDTO(question, ReadAnswers(question, reviewsByMood, mood)));
                }

                items.Add(new AnswerByMoodDTO(mood, questionsWithAnswers));
            }

            return items.OrderByDescending(x => x.Mood).ToList();
        }

        public async Task InsertAsync(Review review)
        {
            if (!review.IsValidated())
            {
                throw new InvalidOperationException("Objeto inválido para gravação.");
            }

            await _storage.Reviews.InsertOneAsync(review);
        }

        public async Task InsertManyAsync(IList<Review> reviews)
        {
            foreach (var review in reviews)
            {
                if (!review.IsValidated())
                {
                    throw new InvalidOperationException("Objeto inválido para gravação.");
                }
            }

            await _storage.Reviews.InsertManyAsync(reviews);
        }

        public async Task EnableAsync(Review review)
        {
            var builder = Builders<Review>.Update.Set(x => x.Active, true);

            await _storage.Reviews.UpdateOneAsync(x => x.Id.Equals(review.Id), builder);
        }

        public async Task DisableAsync(Review review)
        {
            var builder = Builders<Review>.Update.Set(x => x.Active, false);

            await _storage.Reviews.UpdateOneAsync(x => x.Id.Equals(review.Id), builder);
        }

        public async Task<ValidationResultDTO<Review>> ValidateToInsertAsync(Form form, ReviewOnPostDTO dto)
        {
            var result = new ValidationResultDTO<Review>();

            result.ParsedObject.Form = form.Id;
            result.ParsedObject.Date = DateTime.UtcNow;

            #region Mood

            if (!dto.Mood.HasValue)
            {
                result.Error("mood", "Mood não selecionado ou inválido.");
            }
            else
            {
                result.ParsedObject.Mood = dto.Mood.Value;
            }

            #endregion

            #region Tags

            if (form.RequireTagsForReviews)
            {
                if (dto.Tags == null || !dto.Tags.Any())
                {
                    result.Error("tags", "Nenhuma tag foi selecionada.");
                }
                else
                {
                    var tags = await _storage.Tags.Find(x => true).ToListAsync();

                    foreach (var id in dto.Tags)
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
            }

            #endregion

            #region Question

            var questions = await _formsService.GetQuestionsAsync(form);

            if (questions.Any())
            {
                foreach (var question in questions)
                {
                    var answer = dto.Answers?.Where(x => question.Id.ToString() == x.Question).FirstOrDefault();

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

            if (result.Success)
            {
                result.ParsedObject.Validate();
            }

            return result;
        }
    }
}