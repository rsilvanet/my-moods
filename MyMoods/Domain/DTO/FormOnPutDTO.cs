using System.Collections.Generic;

namespace MyMoods.Domain.DTO
{
    public class FormOnPutDTO
    {
        public string MainQuestion { get; set; }
        public IList<string> CustomTags { get; set; }
        public FreeTextDTO FreeText { get; set; }
        public bool AllowMultipleReviewsAtOnce { get; set; }
        public NotificationDTO Notification { get; set; }
    }
}
