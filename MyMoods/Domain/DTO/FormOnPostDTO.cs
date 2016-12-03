namespace MyMoods.Domain.DTO
{
    public class FormOnPostDTO
    {
        public FormOnPostDTO()
        {

        }

        public FormOnPostDTO(string title, bool useDefaultTags)
        {
            Title = title;
            UseDefaultTags = useDefaultTags;
        }

        public string Title { get; set; }
        public bool UseDefaultTags { get; set; }
    }
}
