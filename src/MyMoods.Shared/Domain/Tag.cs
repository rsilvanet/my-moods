﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MyMoods.Shared.Mongo;

namespace MyMoods.Shared.Domain
{
    [BsonIgnoreExtraElements]
    public class Tagg : Entity
    {
        public Tagg()
        {
            Active = true;
        }

        public Tagg(TagType type, string title)
        {
            Type = type;
            Title = title;
            Active = true;
        }

        public bool Active { get; set; }
        public string Title { get; set; }
        public TagType Type { get; set; }
        public ObjectId? Company { get; set; }
        public bool IsCustom => Company != null;
    }
}
