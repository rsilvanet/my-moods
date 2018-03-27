using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace MyMoods.Shared.Mongo
{
    public class Database
    {
        public static IMongoDatabase Get(string connectionString)
        {
            var url = new MongoUrl(connectionString);
            var client = new MongoClient(url);
            var pack = new ConventionPack();

            pack.Add(new CamelCaseElementNameConvention());
            ConventionRegistry.Register("camel case", pack, t => true);

            pack.Add(new EnumRepresentationConvention(BsonType.String));
            ConventionRegistry.Register("EnumStringConvention", pack, t => true);

            return client.GetDatabase(url.DatabaseName);
        }
    }
}
