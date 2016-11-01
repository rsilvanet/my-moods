using MongoDB.Bson;
using MyMoods.Mongo;

namespace MyMoods.Domain
{
    public class Company : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
    }
}
