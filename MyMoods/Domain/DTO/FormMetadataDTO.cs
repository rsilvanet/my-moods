using System.Collections.Generic;
using System.Linq;

namespace MyMoods.Domain.DTO
{
    public class FormMetadataDTO
    {
        public FormMetadataDTO(Form form, Company company, IList<Question> questions, IList<Tagg> tags, IList<MoodDTO> moods)
        {
            Form = new FormWithCompanyDTO(form, company);
            Questions = questions.Select(x => new QuestionDTO(x)).ToList();
            Tags = tags.Select(x => new TagDTO(x)).ToList();
            Moods = moods;
        }

        public FormWithCompanyDTO Form { get; private set; }
        public IList<QuestionDTO> Questions { get; private set; }
        public IList<TagDTO> Tags { get; private set; }
        public IList<MoodDTO> Moods { get; private set; }
    }
}
