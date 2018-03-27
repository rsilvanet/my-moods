using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyMoods.Shared.Mongo;
using System.Collections.Generic;

namespace MyMoods.Shared.Domain
{
    [BsonIgnoreExtraElements]
    public class Question : Entity
    {
        public string Title { get; set; }
        public QuestionType Type { get; set; }
        public bool Required { get; set; }
        public IList<Option> Options { get; set; }
        public ObjectId Form { get; set; }
    }
}
