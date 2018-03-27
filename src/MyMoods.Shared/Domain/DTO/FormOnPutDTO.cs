using System.Collections.Generic;

namespace MyMoods.Shared.Domain.DTO
{
    public class FormOnPutDTO
    {
        public string MainQuestion { get; set; }
        public IList<string> CustomTags { get; set; }
        public bool AllowMultipleReviewsAtOnce { get; set; }
        public FreeTextDTO FreeText { get; set; }
        public NotificationDTO Notification { get; set; }
    }
}
