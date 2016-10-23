using System.Collections.Generic;
using System.Linq;

namespace MyMoods.Domain.DTO
{
    public class QuestionDTO
    {
        public QuestionDTO(Question question)
        {
            Id = question.Id.ToString();
            Title = question.Title;
            Type = question.Type;
            Required = question.Required;
            Options = question.Options?.Select(x => new OptionDTO(x)).ToList();
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public QuestionType Type { get; set; }
        public bool Required { get; set; }
        public IList<OptionDTO> Options { get; set; }

        public class OptionDTO
        {
            public OptionDTO(Option option)
            {
                Value = option.Value;
                Description = option.Description;
            }

            public string Value { get; set; }
            public string Description { get; set; }
        }
    }
}
