using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyMoods.Shared.Mongo
{
    [BsonIgnoreExtraElements]
    public class Entity
    {
        private bool _isValidated;

        public ObjectId Id { get; set; }

        public void Validate()
        {
            _isValidated = true;
        }

        public bool IsValidated()
        {
            return _isValidated;
        }
    }
}
