using System.Collections.Generic;
using System.Linq;

namespace MyMoods.Shared.Domain.DTO
{
    public class FormMetadataDTO
    {
        public FormMetadataDTO(Form form, Company company, IList<MoodDTO> moods)
        {
            Form = new FormWithCompanyDTO(form, company);
            Questions = form.Questions.Select(x => new QuestionDTO(x)).ToList();
            Tags = form.Tags.Select(x => new TagDTO(x)).ToList();
            Moods = moods;
        }

        public FormWithCompanyDTO Form { get; private set; }
        public IList<QuestionDTO> Questions { get; private set; }
        public IList<TagDTO> Tags { get; private set; }
        public IList<MoodDTO> Moods { get; private set; }
    }
}
