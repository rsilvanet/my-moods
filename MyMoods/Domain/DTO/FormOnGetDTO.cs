using System.Collections.Generic;
using System.Linq;

namespace MyMoods.Domain.DTO
{
    public class FormOnGetDTO : FormDTO
    {
        public FormOnGetDTO(Form form) : base(form)
        {
            CustomTags = form.Tags.Where(x => x.IsCustom).Select(x => new TagDTO(x)).ToList();

            var question = form.Questions.FirstOrDefault(x => x.Type == QuestionType.text);

            if (question != null)
            {
                FreeText = new FreeTextDTO(question);
            }
        }

        public IList<TagDTO> CustomTags { get; set; }
        public FreeTextDTO FreeText { get; set; }
    }
}
