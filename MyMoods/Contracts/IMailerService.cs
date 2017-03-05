using MyMoods.Domain;

namespace MyMoods.Contracts
{
    public interface IMailerService
    {
        void Send(string to, string subject, string body);
        void SendResetedPassword(User user, string password);
    }
}
