using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MyMoods.Contracts;
using MyMoods.Domain;
using System.Text;

namespace MyMoods.Services
{
    public class MailerService : IMailerService
    {
        private void Send(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                client.LocalDomain = "mymoods.co";
                client.Connect("mail.privateemail.com", 465, SecureSocketOptions.SslOnConnect);
                client.Authenticate("contato@mymoods.co", "Teste123.");
                client.Send(message);
                client.Disconnect(true);
            }
        }

        private MimeMessage CreateMessage(string to, string subject, string body)
        {
            var mailFrom = new MailboxAddress("contato@mymoods.co");
            var mailTo = new MailboxAddress(to);
            var mailBody = new TextPart("html") { Text = body };

            return new MimeMessage(mailFrom, mailTo, subject, mailBody);
        }

        public void SendResetedPassword(User user, string password)
        {
            var builder = new StringBuilder();
            builder.Append($"<p>Olá {user.Name}</p>");
            builder.Append($"<br>");
            builder.Append($"<p>Sua senha temporária para acesso: <b>{password}</b></p>");
            builder.Append($"<br>");
            builder.Append($"<p>Sugerimos que você altere essa senha por uma de sua preferência através do nosso painel.</p>");
            builder.Append($"<br><br>");
            builder.Append($"Att");
            builder.Append($"<br>");
            builder.Append($"<b>My Moods</b>");

            var message = CreateMessage(user.Email, "Nova senha", builder.ToString());

            Send(message);
        }
    }
}
