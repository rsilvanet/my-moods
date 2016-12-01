using MyMoods.Domain;

namespace MyMoods.Contracts
{
    public interface IMailerService
    {
        void SendResetedPassword(User user, string password);
    }
}
