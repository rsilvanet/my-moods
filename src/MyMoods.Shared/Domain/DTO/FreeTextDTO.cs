namespace MyMoods.Shared.Domain.DTO
{
    public class FreeTextDTO
    {
        public FreeTextDTO()
        {

        }

        public FreeTextDTO(Question question)
        {
            Allow = true;
            Require = question.Required;
            Title = question.Title;
        }

        public bool Allow { get; set; }
        public bool Require { get; set; }
        public string Title { get; set; }
    }
}
