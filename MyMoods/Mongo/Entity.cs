using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyMoods.Mongo
{
    [BsonIgnoreExtraElements]
    public class Entity
    {
        public ObjectId Id { get; set; }
    }
}
