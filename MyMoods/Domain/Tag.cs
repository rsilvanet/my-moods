using MongoDB.Bson;
using MyMoods.Mongo;

namespace MyMoods.Domain
{
    public class Tagg : Entity
    {
        public TagType Type { get; set; }
        public string Title { get; set; }
    }
}
