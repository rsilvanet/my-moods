namespace MyMoods.Domain.DTO
{
    public class FormOnPutDTO
    {
        public FormOnPutDTO()
        {

        }

        public FormOnPutDTO(string title)
        {
            Title = title;
        }

        public string Title { get; set; }
    }
}
