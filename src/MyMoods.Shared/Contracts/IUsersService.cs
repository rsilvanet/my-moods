using MyMoods.Shared.Domain;
using MyMoods.Shared.Domain.DTO;
using System.Threading.Tasks;

namespace MyMoods.Shared.Contracts
{
    public interface IUsersService
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<User> AuthenticateAsync(string email, string password);
        Task InsertAsync(Company company, User user);
        Task ResetPasswordAsync(User user);
        Task ChangePasswordAsync(User user, string password);
        Task<ValidationResultDTO<User>> ValidateToInsertAsync(RegisterDTO register);
        Task<ValidationResultDTO> ValidateToChangePasswordAsync(User user, ChagePasswordDTO dto);
    }
}
