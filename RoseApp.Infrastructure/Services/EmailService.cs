using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using RoseApp.Application.Contracts.IServices;

namespace RoseApp.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(
                emailSettings["SenderName"],
                emailSettings["SenderEmail"]));

            email.To.Add(MailboxAddress.Parse(to));

            email.Subject = subject;

            email.Body = new TextPart("html") { Text = body };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                emailSettings["Host"],
                int.Parse(emailSettings["Port"]!),
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                emailSettings["SenderEmail"],
                emailSettings["Password"]);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
