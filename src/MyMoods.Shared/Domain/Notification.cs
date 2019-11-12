using MongoDB.Bson.Serialization.Attributes;
using MyMoods.Shared.Domain.Enums;
using System.Collections.Generic;

namespace MyMoods.Shared.Domain
{
    [BsonIgnoreExtraElements]
    public class Notification
    {
        public Notification() { }

        public Notification(NotificationType type)
        {
            Type = type;
        }

        public bool Active { get; set; }
        public NotificationType Type { get; set; }
        public NotificationRecurrence Recurrence { get; set; }
        public IList<Contact> To { get; set; }
    }
}
