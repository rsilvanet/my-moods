using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyMoods.Mongo;

namespace MyMoods.Domain
{
    [BsonIgnoreExtraElements]
    public class Form : Entity
    {
        public Form()
        {
            Active = true;
        }

        public string Title { get; set; }
        public bool UseCustomTags { get; set; }
        public bool UseDefaultTags { get; set; }
        public bool Active { get; set; }
        public ObjectId Company { get; set; }
    }
}
