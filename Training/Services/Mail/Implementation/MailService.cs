using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using UserManagementSystem.Models;
using static UserManagementSystem.Helper.EmailTemplate;

namespace UserManagementSystem.Services.Mail.Implementation
{
    public class MailService : IMailService
    {

        private readonly IConfiguration _mailConfiguration;
        private readonly IConfiguration _appConfiguration;
        private readonly UserManager<User> _userManager;
        public MailService(IConfiguration configuration,UserManager<User> userManager)
        {
            _mailConfiguration = configuration.GetSection("MailSettings");
            _appConfiguration = configuration.GetSection("AppSettings");
            _userManager = userManager;
        }

        public async Task SendMailAsync(string recipient, string subject, string body)
        {
            // Connect to SMTP server
            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(
                    _mailConfiguration["Host"]!,
                    int.Parse(_mailConfiguration["Port"]!),
                    MailKit.Security.SecureSocketOptions.StartTls
                );

                // Authenticate SMTP client
                await smtp.AuthenticateAsync(
                    _mailConfiguration["From"]!,
                    _mailConfiguration["Password"]!
                );

                // Create email message  
                var message = new MimeMessage();

                // Set sender and recipient
                message.From.Add(new MailboxAddress(
                    _mailConfiguration["UserName"]!,
                    _mailConfiguration["From"]!
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

        public async Task SendConfirmationEmailAsync(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);

            var baseURL = _appConfiguration["BaseUrl"];

            var confirmationLink = $"{baseURL}/api/Auth/confirm-email?userId={user.Id}&token={encodedToken}";

            var subject = GetConfirmationEmailSubject();

            var body = GetConfirmationEmailBody(confirmationLink);

            await SendMailAsync(user.Email!,subject,body);
        }
    }
}
