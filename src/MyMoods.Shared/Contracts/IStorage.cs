using MongoDB.Driver;
using MyMoods.Shared.Domain;

namespace MyMoods.Shared.Contracts
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
