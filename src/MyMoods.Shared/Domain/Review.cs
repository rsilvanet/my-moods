using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyMoods.Shared.Domain.Enums;
using MyMoods.Shared.Mongo;
using System;
using System.Collections.Generic;

namespace MyMoods.Shared.Domain
{
    [BsonIgnoreExtraElements]
    public class Review : Entity
    {
        public Review()
        {
            Active = true;
            Date = DateTime.UtcNow;
            Tags = new List<ObjectId>();
            Answers = new List<Answer>();
        }

        public bool Active { get; set; }
        public DateTime Date { get; set; }
        public MoodType Mood { get; set; }
        public IList<ObjectId> Tags { get; set; }
        public IList<Answer> Answers { get; set; }
        public ObjectId Form { get; set; }
    }
}
