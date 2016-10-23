using System.Collections.Generic;
using System.Linq;

namespace MyMoods.Domain.DTO
{
    public class FormMetadataDTO
    {
        public FormMetadataDTO(Form form, Company company, IList<Question> questions, IList<Tagg> tags)
        {
            Form = new
            {
                Id = form.Id.ToString(),
                Title = form.Title,
                Company = new
                {
                    Name = company.Name
                }
            };

            Questions = questions.Select(x => new
            {
                Id = x.Id.ToString(),
                Title = x.Title,
                Type = x.Type,
                Required = x.Required,
                Options = x.Options
            });

            Tags = tags.Select(x => new
            {
                Id = x.Id.ToString(),
                Title = x.Title,
                Type = x.Type
            });
        }

        public object Form { get; private set; }
        public object Moods { get; private set; }
        public object Questions { get; private set; }
        public object Tags { get; private set; }
    }
}
