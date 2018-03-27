using Hangfire;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MyMoods.Shared.Contracts;
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
            var mailFrom = new List<MailboxAddress>() { new MailboxAddress("My Moods", "contato@mymoods.com.br") };
            var mailBody = new TextPart("html") { Text = body };

            return new MimeMessage(mailFrom, mailTo, subject, mailBody);
        }

        public void Send(string to, string subject, string body)
        {
            var message = CreateMessage(to, subject, body);

            using (var client = new SmtpClient())
            {
                var section = _settings.GetSection("Email");
                var host = section.GetSection("Host").Value;
                var port = int.Parse(section.GetSection("Port").Value);
                var user = section.GetSection("Username").Value;
                var pass = section.GetSection("Password").Value;

                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(host, port, false);
                client.AuthenticationMechanisms.Remove ("XOAUTH2");
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
