using MyMoods.Domain;
using System.Threading.Tasks;

namespace MyMoods.Contracts
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string email, string password);
    }
}
