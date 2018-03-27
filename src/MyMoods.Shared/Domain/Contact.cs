using MongoDB.Bson.Serialization.Attributes;

namespace MyMoods.Shared.Domain
{
    [BsonIgnoreExtraElements]
    public class Contact
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        public string Url { get; set; }
    }
}
