namespace MyMoods.Domain.DTO
{
    public class FormDTO
    {
        public FormDTO(Form form)
        {
            Id = form.Id.ToString();
            Title = form.Title;
        }

        public string Id { get; set; }
        public string Title { get; set; }
    }
}
