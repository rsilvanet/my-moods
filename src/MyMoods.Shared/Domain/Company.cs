using MongoDB.Bson.Serialization.Attributes;
using MyMoods.Shared.Mongo;

namespace MyMoods.Shared.Domain
{
    [BsonIgnoreExtraElements]
    public class Company : Entity
    {
        public Company()
        {
            Active = true;
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool Active { get; set; }
    }
}
