using MailKit.Net.Smtp;
using MimeKit;

namespace Training.Services.Mail.Implementation
{
    public class MailService : IMailService
    {

        private readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string recipient, string subject, string body)
        {
            // Connect to SMTP server
            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(
                    _configuration["MailSettings:Host"]!,
                    int.Parse(_configuration["MailSettings:Port"]!),
                    MailKit.Security.SecureSocketOptions.StartTls
                );

                // Authenticate SMTP client
                await smtp.AuthenticateAsync(
                    _configuration["MailSettings:UserName"]!,
                    _configuration["MailSettings:Password"]!
                );

                // Create email message  
                var message = new MimeMessage();

                // Set sender and recipient
                message.From.Add(new MailboxAddress(
                    "Hamza Sagheer",
                    _configuration["MailSettings:From"]!
                ));
                message.To.Add(MailboxAddress.Parse(recipient));

                // Set subject and body
                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = body
                };

                // Send email
                await smtp.SendAsync(message);

                // Disconnect from SMTP server
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
