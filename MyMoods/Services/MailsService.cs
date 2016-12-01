using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MyMoods.Contracts;
using MyMoods.Domain;
using System.Threading.Tasks;

namespace MyMoods.Services
{
    public class MailsService : IMailsService
    {
        public async Task SendNewPasswordAsync(User user)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("contato@mymoods.co"));
            message.To.Add(new MailboxAddress(user.Email));
            message.Subject = "Nova senha";
            message.Body = new TextPart("html")
            {
                Text = "<p>Olá {user.Name}.</p>"
            };
            
            using (var client = new SmtpClient())
            {
                client.LocalDomain = "mymoods.co";
                await client.ConnectAsync("smtp.relay.uri", 25, SecureSocketOptions.None).ConfigureAwait(false);
                await client.SendAsync(message).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}
