using System.Collections.Generic;

namespace MyMoods.Shared.Domain.DTO
{
    public class QuestionWithAnswersDTO : QuestionDTO
    {
        public QuestionWithAnswersDTO(Question question, IList<string> answers) : base(question)
        {
            Answers = answers;
        }

        public IList<string> Answers { get; set; }
    }
}
