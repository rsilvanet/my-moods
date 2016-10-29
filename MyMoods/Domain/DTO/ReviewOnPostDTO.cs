using System.Collections.Generic;

namespace MyMoods.Domain.DTO
{
    public class ReviewOnPostDTO
    {
        public string Mood { get; set; }
        public IList<string> Tags { get; set; }
        public IList<AnswerDTO> Answers { get; set; }

        public class AnswerDTO
        {
            public string Value { get; set; }
            public string Question { get; set; }
        }
    }
}
