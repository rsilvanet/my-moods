using MongoDB.Bson.Serialization.Attributes;
using MyMoods.Mongo;

namespace MyMoods.Domain
{
    [BsonIgnoreExtraElements]
    public class Tagg : Entity
    {
        public Tagg()
        {
            Active = true;
        }

        public Tagg(TagType type, string title)
        {
            Type = type;
            Title = title;
            Active = true;
        }

        public TagType Type { get; set; }
        public string Title { get; set; }
        public bool Active { get; set; }
    }
}
