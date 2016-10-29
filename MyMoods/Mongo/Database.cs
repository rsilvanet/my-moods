using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace MyMoods.Mongo
{
    public class Database
    {
        public static IMongoDatabase Get()
        {
            var url = new MongoUrl("mongodb://admin:admin@ds029665.mlab.com:29665/youtalkme-mongo");
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
