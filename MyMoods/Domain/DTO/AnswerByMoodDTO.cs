using System.Collections.Generic;

namespace MyMoods.Domain.DTO
{
    public class AnswerByMoodDTO
    {
        public AnswerByMoodDTO(MoodType mood, IList<QuestionWithAnswersDTO> questions)
        {
            Mood = mood;
            Questions = questions;
        }

        public MoodType Mood { get; set; }
        public IList<QuestionWithAnswersDTO> Questions { get; set; }
    }
}
