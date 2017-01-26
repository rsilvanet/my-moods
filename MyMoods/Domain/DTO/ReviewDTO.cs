using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMoods.Domain.DTO
{
    public class ReviewDTO
    {
        public ReviewDTO(Review review, IList<Tagg> tags)
        {
            Id = review.Id.ToString();
            Active = review.Active;
            Date = review.Date;
            Mood = review.Mood;
            Tags = new List<TagDTO>();
            Answers = review.Answers.Select(x => new AnswerDTO(x)).ToList();

            foreach (var tagId in review.Tags)
            {
                var tag = tags.FirstOrDefault(x => x.Id.ToString() == tagId.ToString());

                if (tag != null)
                {
                    Tags.Add(new TagDTO(tag));
                }
            }
        }

        public string Id { get; set; }
        public bool Active { get; set; }
        public DateTime Date { get; set; }
        public MoodType Mood { get; set; }
        public IList<TagDTO> Tags { get; set; }
        public IList<AnswerDTO> Answers { get; set; }

        public class AnswerDTO
        {
            public AnswerDTO(Answer answer)
            {
                Value = answer.Value;
                Question = answer.Question.ToString();
            }

            public string Value { get; set; }
            public string Question { get; set; }
        }
    }
}
