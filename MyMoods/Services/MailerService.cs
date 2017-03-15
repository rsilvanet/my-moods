using Hangfire;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MyMoods.Contracts;
using System.Collections.Generic;

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

        public void Enqueue(string to, string subject, string body)
        {
            BackgroundJob.Enqueue<IMailerService>(x => x.Send(to, subject, body));
        }
    }
}
