using System.Collections.Generic;

namespace MyMoods.Domain.DTO
{
    public class FormOnPostDTO
    {
        public string Title { get; set; }
        public string MainQuestion { get; set; }
        public FormType? Type { get; set; }
        public IList<string> CustomTags { get; set; }
        public FreeTextDTO FreeText { get; set; }

        public class FreeTextDTO
        {
            public bool Allow { get; set; }
            public bool Require { get; set; }
            public string Title { get; set; }
        }
    }
}
