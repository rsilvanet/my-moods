using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyMoods.Mongo
{
    [BsonIgnoreExtraElements]
    public class Entity
    {
        private bool _validatedWasCalled;

        public ObjectId Id { get; set; }

        public void Validate()
        {
            _validatedWasCalled = true;
        }

        public bool ValidateWasCalled()
        {
            return _validatedWasCalled;
        }
    }
}
