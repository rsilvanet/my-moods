using MongoDB.Driver;
using MyMoods.Contracts;
using MyMoods.Domain;

namespace MyMoods.Mongo
{
    public class Storage : IStorage
    {
        private readonly IMongoDatabase _database;

        public Storage(IMongoDatabase database)
        {
            _database = database;
        }

        public IMongoCollection<Company> Companies => _database.GetCollection<Company>("companies");
        public IMongoCollection<Form> Forms => _database.GetCollection<Form>("forms");
        public IMongoCollection<Question> Questions => _database.GetCollection<Question>("questions");
        public IMongoCollection<Review> Reviews => _database.GetCollection<Review>("reviews");
        public IMongoCollection<Tagg> Tags => _database.GetCollection<Tagg>("tags");
        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
    }
}
