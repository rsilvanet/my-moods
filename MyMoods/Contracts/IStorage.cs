using MongoDB.Driver;
using MyMoods.Domain;

namespace MyMoods.Contracts
{
    public interface IStorage
    {
        IMongoCollection<Company> Companies { get; }
        IMongoCollection<Form> Forms { get; }
        IMongoCollection<Question> Questions { get; }
        IMongoCollection<Review> Reviews { get; }
        IMongoCollection<Tagg> Tags { get; }
        IMongoCollection<User> Users { get; }
    }
}
