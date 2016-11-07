using MongoDB.Bson.Serialization.Attributes;
using MyMoods.Mongo;
using System;

namespace MyMoods.Domain
{
    [BsonIgnoreExtraElements]
    public class Company : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
