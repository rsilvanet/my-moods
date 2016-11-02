using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyMoods.Mongo;

namespace MyMoods.Domain
{
    [BsonIgnoreExtraElements]
    public class Form : Entity
    {
        public string Title { get; set; }
        public ObjectId Company { get; set; }
    }
}
