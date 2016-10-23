using MongoDB.Bson;

namespace MyMoods.Domain
{
    public class Form
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public ObjectId Company { get; set; }
    }
}
