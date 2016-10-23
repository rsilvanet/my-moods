using MongoDB.Bson;
using System.Collections.Generic;

namespace MyMoods.Domain
{
    public class Question
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public QuestionType Type { get; set; }
        public bool Required { get; set; }
        public IList<Option> Options { get; set; }
        public ObjectId Form { get; set; }
    }
}
