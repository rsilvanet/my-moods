using MongoDB.Bson;
using MyMoods.Mongo;

namespace MyMoods.Domain
{
    public class Form : Entity
    {
        public string Title { get; set; }
        public ObjectId Company { get; set; }
    }
}
