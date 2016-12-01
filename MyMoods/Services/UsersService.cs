using MongoDB.Bson;
using MongoDB.Driver;
using MyMoods.Contracts;
using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyMoods.Services
{
    public class UsersService : IUsersService
    {
        private readonly IStorage _storage;
        private readonly IMailerService _mailer;

        public UsersService(IStorage storage, IMailerService mailer)
        {
            _storage = storage;
            _mailer = mailer;
        }

        private string CryptoPass(string password)
        {
            password = $"x8n5pT.{password}.3i1klM";

            var md5 = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(bytes);

            var builder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }

            return builder.ToString();
        }

        private string GeneratePass()
        {
            var guid = Guid.NewGuid().ToString();
            var pass = $"{guid.Substring(0, 5)}{guid.Substring(10, 5)}{guid.Substring(20, 5)}";

            return pass.Replace("-", "");
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            password = CryptoPass(password);

            return await _storage.Users.Find(x => x.Email == email && (x.Password == password || x.ResetedPassword == password)).FirstOrDefaultAsync();
        }

        public async Task ResetPasswordAsync(User user)
        {
            var pass = GeneratePass();
            var cryptoPass = CryptoPass(pass);
            var builder = Builders<User>.Update.Set(x => x.ResetedPassword, cryptoPass);

            await _storage.Users.UpdateOneAsync(x => x.Id.Equals(user.Id), builder);

            _mailer.SendResetedPassword(user, pass);
        }

        public async Task InsertAsync(Company company, User user)
        {
            user.Password = CryptoPass(user.Password);
            user.Companies = new List<ObjectId> { company.Id };

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
