using MongoDB.Driver;
using MyMoods.Contracts;
using MyMoods.Domain;
using System.Threading.Tasks;

namespace MyMoods.Services
{
    public class UserService : IUserService
    {
        private readonly IStorage _storage;

        public UserService(IStorage storage)
        {
            _storage = storage;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            return await _storage.Users.Find(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();
        }
    }
}
