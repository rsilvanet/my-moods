using MongoDB.Bson;
using MongoDB.Driver;
using MyMoods.Contracts;
using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMoods.Services
{
    public class UsersService : IUsersService
    {
        private readonly IStorage _storage;

        public UsersService(IStorage storage)
        {
            _storage = storage;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            return await _storage.Users.Find(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();
        }

        public async Task InsertAsync(Company company, User user)
        {
            user.Companies = new List<ObjectId>
            {
                company.Id
            };

            await _storage.Users.InsertOneAsync(user);
        }

        public async Task<ValidationResultDTO<User>> ValidateToInsertAsync(RegisterDTO register)
        {
            var result = new ValidationResultDTO<User>();

            if (string.IsNullOrWhiteSpace(register.Name))
            {
                result.Error("name", "O nome do usuário não foi informado.");
            }
            else
            {
                result.ParsedObject.Name = register.Name;
            }

            if (string.IsNullOrWhiteSpace(register.Email))
            {
                result.Error("email", "O e-mail do usuário não foi informado.");
            }
            else
            {
                var user = await _storage.Users.Find(x => x.Email.ToLower() == register.Email.ToLower()).FirstOrDefaultAsync();

                if (user != null)
                {
                    result.Error("email", "Já existe um usuário cadastrado com este e-mail.");
                }
                else
                {
                    result.ParsedObject.Email = register.Email;
                }
            }

            if (string.IsNullOrWhiteSpace(register.Password))
            {
                result.Error("password", "A senha do usuário não foi informada.");
            }
            else
            {
                result.ParsedObject.Password = register.Password;
            }

            return result;
        }
    }
}
