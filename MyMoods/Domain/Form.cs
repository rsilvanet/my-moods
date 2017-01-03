using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyMoods.Mongo;
using System.Collections.Generic;

namespace MyMoods.Domain
{
    [BsonIgnoreExtraElements]
    public class Form : Entity
    {
        public Form()
        {
            Active = true;
            CustomTags = new List<ObjectId>();
        }

        public Form(ObjectId company)
        {
            Active = true;
            Company = company;
            CustomTags = new List<ObjectId>();
        }

        public bool Active { get; set; }
        public string Title { get; set; }
        public string MainQuestion { get; set; }
        public FormType Type { get; set; }
        public IList<ObjectId> CustomTags { get; set; }
        public ObjectId Company { get; set; }

        public bool RequireTagsForReviews => Type != FormType.simple;
    }
}
