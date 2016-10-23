using MongoDB.Bson;

namespace MyMoods.Domain
{
    public class Answer
    {
        public ObjectId Question { get; set; }
        public string Value { get; set; }
    }
}
