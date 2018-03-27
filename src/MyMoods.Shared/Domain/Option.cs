using MongoDB.Bson.Serialization.Attributes;

namespace MyMoods.Shared.Domain
{
    [BsonIgnoreExtraElements]
    public class Option
    {
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
