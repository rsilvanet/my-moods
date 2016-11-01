using MongoDB.Bson;
using MyMoods.Mongo;
using System.Collections.Generic;

namespace MyMoods.Domain
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; }
        public IList<ObjectId> Companies { get; set; }
    }
}
