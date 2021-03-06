﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyMoods.Shared.Mongo;
using System.Collections.Generic;

namespace MyMoods.Shared.Domain
{
    [BsonIgnoreExtraElements]
    public class User : Entity
    {
        public User()
        {
            Active = true;
        }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ResetedPassword { get; set; }
        public bool Admin { get; set; }
        public bool Active { get; set; }
        public IList<ObjectId> Companies { get; set; }
    }
}
