using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyMoods.Domain
{
    [BsonIgnoreExtraElements]
    public class Answer
    {
        public Answer()
        {

        }

        public Answer(Question question)
        {
            Question = question.Id;
        }

        public ObjectId Question { get; set; }
        public string Value { get; set; }
    }
}
