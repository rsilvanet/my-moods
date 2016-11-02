using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyMoods.Mongo;

namespace MyMoods.Domain
{
    [BsonIgnoreExtraElements]
    public class Company : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
    }
}
