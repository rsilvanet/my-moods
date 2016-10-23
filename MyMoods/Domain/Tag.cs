using MongoDB.Bson;

namespace MyMoods.Domain
{
    public class Tagg
    {
        public ObjectId Id { get; set; }
        public TagType Type { get; set; }
        public string Title { get; set; }
        public object __v { get; set; }
    }
}
