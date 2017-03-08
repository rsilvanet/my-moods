namespace MyMoods.Domain.DTO
{
    public class FormDTO
    {
        public FormDTO(Form form)
        {
            Id = form.Id.ToString();
            Title = form.Title;
            MainQuestion = form.MainQuestion;
            Type = form.Type;
            Active = form.Active;
            AllowMultipleReviewsAtOnce = form.AllowMultipleReviewsAtOnce;
            UseTags = form.RequireTagsForReviews;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string MainQuestion { get; set; }
        public FormType Type { get; set; }
        public bool Active { get; set; }
        public bool AllowMultipleReviewsAtOnce { get; set; }
        public bool UseTags { get; set; }
    }
}
