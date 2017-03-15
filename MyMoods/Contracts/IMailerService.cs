namespace MyMoods.Contracts
{
    public interface IMailerService
    {
        void Send(string to, string subject, string body);
        void Enqueue(string to, string subject, string body);
    }
}
