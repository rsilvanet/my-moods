using MyMoods.Domain;

namespace MyMoods.Contracts
{
    public interface IMailerService
    {
        void SendNewPassword(User user);
    }
}
