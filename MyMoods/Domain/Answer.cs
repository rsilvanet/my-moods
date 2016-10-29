using MongoDB.Bson;

namespace MyMoods.Domain
{
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
