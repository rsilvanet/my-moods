using System.Linq;

namespace MyMoods.Shared.Domain.DTO
{
    public class NotificationDTO
    {
        public NotificationDTO()
        {
            Active = false;
        }

        public NotificationDTO(Notification notification)
        {
            Active = notification.Active;
            Email = notification.To?.FirstOrDefault()?.Email;
            Recurrence = notification.Recurrence;
        }

        public bool Active { get; set; }
        public string Email { get; set; }
        public NotificationRecurrence? Recurrence { get; set; }
    }
}
