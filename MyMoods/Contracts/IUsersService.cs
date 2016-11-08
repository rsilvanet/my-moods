using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Threading.Tasks;

namespace MyMoods.Contracts
{
    public interface IUsersService
    {
        Task<User> AuthenticateAsync(string email, string password);
        Task InsertAsync(Company company, User user);
        Task<ValidationResultDTO<User>> ValidateToInsertAsync(RegisterDTO register);
    }
}
