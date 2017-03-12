namespace MyMoods.Domain.DTO
{
    public class NotificationDTO
    {
        public bool Active { get; set; }
        public string Email { get; set; }
        public NotificationRecurrence? Recurrence { get; set; }
    }
}
