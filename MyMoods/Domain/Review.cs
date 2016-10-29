using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace MyMoods.Domain
{
    public class Review
    {
        public Review()
        {
            Tags = new List<ObjectId>();
            Answers = new List<Answer>();
        }

        public ObjectId Id { get; set; }
        public DateTime Date { get; set; }
        public MoodType Mood { get; set; }
        public IList<ObjectId> Tags { get; set; }
        public IList<Answer> Answers { get; set; }
        public ObjectId Form { get; set; }
    }
}
