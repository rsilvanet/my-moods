using MongoDB.Bson;
using System.Collections.Generic;

namespace MyMoods.Domain
{
    public class User
    {
        public ObjectId Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool Administrator { get; set; }
        public IList<ObjectId> Companies { get; set; }
    }
}
