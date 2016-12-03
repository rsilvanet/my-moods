using MyMoods.Domain;
using MyMoods.Domain.DTO;
using System.Threading.Tasks;

namespace MyMoods.Contracts
{
    public interface IUsersService
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<User> AuthenticateAsync(string email, string password);
        Task ResetPasswordAsync(User user);
        Task ChangePasswordAsync(User user, string password);
        Task<ValidationResultDTO> ValidateToChangePasswordAsync(User user, ChagePasswordDTO dto);
        Task InsertAsync(Company company, User user);
        Task<ValidationResultDTO<User>> ValidateToInsertAsync(RegisterDTO register);
    }
}
