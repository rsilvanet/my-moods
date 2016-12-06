using System.Collections.Generic;
using System.Linq;

namespace MyMoods.Domain.DTO
{
    public class FormWithQuestionsDTO : FormDTO
    {
        public FormWithQuestionsDTO(Form form, IList<Question> questions) : base(form)
        {
            Questions = questions.Select(x => new QuestionDTO(x)).ToList();
        }

        public IList<QuestionDTO> Questions { get; set; }

        public class QuestionDTO
        {
            public QuestionDTO(Question question)
            {
                Id = question.Id.ToString();
                Title = question.Title;
            }

            public string Id { get; set; }
            public string Title { get; set; }
        }
    }
}
