using System.Collections.Generic;

namespace MyMoods.Shared.Domain.DTO
{
    public class ReviewsDetailedByMoodDTO
    {
        public MoodType Mood { get; set; }
        public int Count { get; set; }
        public IList<TagCounterDTO> Tags { get; set; }
        public IList<QuestionWithAnswersDTO> Questions { get; set; }
    }
}
