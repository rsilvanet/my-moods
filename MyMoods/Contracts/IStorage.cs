using MongoDB.Driver;

namespace MyMoods.Contracts
{
    public interface IStorage
    {
        IMongoCollection<Domain.Company> Companies { get; }
        IMongoCollection<Domain.Form> Forms { get; }
        IMongoCollection<Domain.Question> Questions { get; }
        IMongoCollection<Domain.Review> Reviews { get; }
        IMongoCollection<Domain.Tagg> Tags { get; }
    }
}
