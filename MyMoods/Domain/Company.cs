using MongoDB.Bson;

namespace MyMoods.Domain
{
    public class Company
    {
        public ObjectId Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
