using MongoDB.Bson;
using MyMoods.Mongo;
using System.Collections.Generic;

namespace MyMoods.Domain
{
    public class Question : Entity
    {
        public string Title { get; set; }
        public QuestionType Type { get; set; }
        public bool Required { get; set; }
        public IList<Option> Options { get; set; }
        public ObjectId Form { get; set; }
    }
}
