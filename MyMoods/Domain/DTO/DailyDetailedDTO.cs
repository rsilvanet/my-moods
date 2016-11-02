using System.Collections.Generic;

namespace MyMoods.Domain.DTO
{
    public class DailyDetailedDTO
    {
        public MoodType Mood { get; set; }
        public string Image { get; set; }
        public int Count { get; set; }
        public IList<TagDTO> Tags { get; set; }
        public IList<QuestionWithAnswersDTO> Questions { get; set; }
    }
}
