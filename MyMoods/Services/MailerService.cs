using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MyMoods.Contracts;
using MyMoods.Domain;
using System.Collections.Generic;
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
            var mailTo = new List<MailboxAddress>() { new MailboxAddress(to) };
            var mailFrom = new List<MailboxAddress>() { new MailboxAddress("contato@mymoods.co") };
            var mailBody = new TextPart("html") { Text = body };

            return new MimeMessage(mailFrom, mailTo, subject, mailBody);
        }

        public void SendResetedPassword(User user, string password)
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

            var message = CreateMessage(user.Email, "Nova senha", builder.ToString());

            Send(message);
        }
    }
}
