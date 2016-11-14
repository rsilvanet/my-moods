using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyMoods.Mongo;

namespace MyMoods.Domain
{
    [BsonIgnoreExtraElements]
    public class Tagg : Entity
    {
        public Tagg()
        {
        }

        public Tagg(TagType type, string title)
        {
            Type = type;
            Title = title;
        }

        public TagType Type { get; set; }
        public string Title { get; set; }
    }
}
