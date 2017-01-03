namespace MyMoods.Domain.DTO
{
    public class FormOnPostDTO
    {
        public FormOnPostDTO()
        {

        }

        public FormOnPostDTO(string title)
        {
            Title = title;
        }

        public string Title { get; set; }
    }
}
