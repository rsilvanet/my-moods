using MongoDB.Bson;

namespace MyMoods.Mongo
{
    public class Entity
    {
        public ObjectId Id { get; set; }
        public object __v { get; set; }
    }
}
