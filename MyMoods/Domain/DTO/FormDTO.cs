namespace MyMoods.Domain.DTO
{
    public class FormDTO
    {
        public FormDTO(Form form)
        {
            Id = form.Id.ToString();
            Title = form.Title;
            Type = form.Type;
            Active = form.Active;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public FormType Type { get; set; }
        public bool Active { get; set; }
    }
}
