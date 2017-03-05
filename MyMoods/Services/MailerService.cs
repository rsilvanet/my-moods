using Hangfire;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MyMoods.Contracts;
using MyMoods.Domain;
using System.Collections.Generic;
using System.Text;

namespace MyMoods.Services
{
    public class MailerService : IMailerService
    {
        private readonly IConfigurationRoot _settings;

        public MailerService(IConfigurationRoot settings)
        {
            _settings = settings;
        }

        private MimeMessage CreateMessage(string to, string subject, string body)
        {
            var mailTo = new List<MailboxAddress>() { new MailboxAddress(to) };
            var mailFrom = new List<MailboxAddress>() { new MailboxAddress("My Moods", "contato@mymoods.co") };
            var mailBody = new TextPart("html") { Text = body };

            return new MimeMessage(mailFrom, mailTo, subject, mailBody);
        }

        public void Send(string to, string subject, string body)
        {
            var message = CreateMessage(to, subject, body);

            using (var client = new SmtpClient())
            {
                var section = _settings.GetSection("Email");
                var host = section.GetValue<string>("Host");
                var port = section.GetValue<int>("Port");
                var user = section.GetValue<string>("Username");
                var pass = section.GetValue<string>("Password");

                client.Connect(host, port, SecureSocketOptions.SslOnConnect);
                client.Authenticate(user, pass);
                client.Send(message);
                client.Disconnect(true);
            }
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

            BackgroundJob.Enqueue<IMailerService>(x => x.Send(user.Email, "Nova senha", builder.ToString()));
        }
    }
}
