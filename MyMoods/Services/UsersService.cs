using Hangfire;
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

        private bool CheckPass(User user, string password)
        {
            password = CryptoPass(password);

            return user.Password == password || user.ResetedPassword == password;
        }

        private void SendResetedPassword(User user, string password)
        {
            var builder = new StringBuilder();
            builder.Append($"Olá {user.Name}.");
            builder.Append($"<br><br>");
            builder.Append($"Conforme solicitado, geramos uma senha temporária de acesso para você.");
            builder.Append($"<br>");
            builder.Append($"Sugerimos que você altere essa senha por uma de sua preferência através do nosso painel.");
            builder.Append($"<br><br>");
            builder.Append($"Senha temporária: <b>{password}</b>");
            builder.Append($"<br><br>");
            builder.Append($"Clique <a href='mymoods.co/analytics/#/login' target='_blank'>aqui</a> para acessar.");
            builder.Append($"<br><br>");
            builder.Append($"Att");
            builder.Append($"<br>");
            builder.Append($"<b>My Moods</b>");

            _mailer.Enqueue(user.Email, "Nova senha", builder.ToString());
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var oid = new ObjectId(id);
            var user = await _storage.Users.Find(x => x.Id.Equals(oid)).FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _storage.Users.Find(x => x.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await GetByEmailAsync(email);

            if (user != null && CheckPass(user, password))
            {
                return user;
            }

            return null;
        }

        public async Task InsertAsync(Company company, User user)
        {
            user.Password = CryptoPass(user.Password);
            user.Companies = new List<ObjectId> { company.Id };

            await _storage.Users.InsertOneAsync(user);
        }

        public async Task ResetPasswordAsync(User user)
        {
            var pass = GeneratePass();
            var cryptoPass = CryptoPass(pass);
            var builder = Builders<User>.Update.Set(x => x.ResetedPassword, cryptoPass);

            await _storage.Users.UpdateOneAsync(x => x.Id.Equals(user.Id), builder);

            SendResetedPassword(user, pass);
        }

        public async Task ChangePasswordAsync(User user, string password)
        {
            var cryptoPass = CryptoPass(password);

            var builder = Builders<User>.Update
                .Set(x => x.Password, cryptoPass)
                .Set(x => x.ResetedPassword, null);

            await _storage.Users.UpdateOneAsync(x => x.Id.Equals(user.Id), builder);
        }

        public async Task<ValidationResultDTO<User>> ValidateToInsertAsync(RegisterDTO dto)
        {
            var result = new ValidationResultDTO<User>();

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                result.Error("name", "O nome do usuário não foi informado.");
            }
            else
            {
                result.ParsedObject.Name = dto.Name;
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                result.Error("email", "O e-mail do usuário não foi informado.");
            }
            else
            {
                var user = await GetByEmailAsync(dto.Email);

                if (user != null)
                {
                    result.Error("email", "Já existe um usuário cadastrado com este e-mail.");
                }
                else
                {
                    result.ParsedObject.Email = dto.Email;
                }
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                result.Error("password", "A senha do usuário não foi informada.");
            }
            else
            {
                result.ParsedObject.Password = dto.Password;
            }

            return result;
        }

        public Task<ValidationResultDTO> ValidateToChangePasswordAsync(User user, ChagePasswordDTO dto)
        {
            var result = new ValidationResultDTO();

            if (string.IsNullOrWhiteSpace(dto.Old))
            {
                result.Error("old", "A senha atual não foi informada.");
            }
            else if (!CheckPass(user, dto.Old))
            {
                result.Error("old", "A senha atual está incorreta.");
            }

            if (string.IsNullOrWhiteSpace(dto.New))
            {
                result.Error("new", "A nova senha não foi informada.");
            }
            else if (dto.New.Length < 6)
            {
                result.Error("new", "A nova senha deve ter no mínimo 6 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(dto.Confirmation))
            {
                result.Error("confirmation", "A confirmação não foi informada");
            }
            else if (dto.New != dto.Confirmation)
            {
                result.Error("confirmation", "A confirmação não coincide com a senha.");
            }

            return Task.FromResult(result);
        }
    }
}
