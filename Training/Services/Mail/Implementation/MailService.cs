using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Serilog.Context;
using UserManagementSystem.Models;
using static UserManagementSystem.Helper.EmailTemplate;

namespace UserManagementSystem.Services.Mail.Implementation
{
    public class MailService : IMailService
    {

        private readonly IConfiguration _mailConfiguration;
        private readonly IConfiguration _urlConfiguration;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<MailService> _logger;
        public MailService(IConfiguration configuration,UserManager<User> userManager,
            ILogger<MailService> logger)
        {
            _mailConfiguration = configuration.GetSection("MailSettings");
            _urlConfiguration = configuration.GetSection("Urls");
            _userManager = userManager;
            _logger = logger;
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

            using var _uid = LogContext.PushProperty("UserId", user.Id);
            using var _un = LogContext.PushProperty("UserName", user.UserName);

            try
            {
                _logger.LogInformation("Confirmation email sending started. Email: {Email}", user.Email);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = Uri.EscapeDataString(token);

                var confirmationLink = $"{_urlConfiguration["BackendUrl"]}/api/Auth/confirm-email?userId={user.Id}&token={encodedToken}";

                var subject = GetConfirmationEmailSubject();

                var body = GetConfirmationEmailBody(confirmationLink);

                await SendMailAsync(user.Email!, subject, body);

                _logger.LogInformation("Confirmation email sent successfully. Email: {Email}", user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send confirmation email. Email: {Email}", user.Email);
                throw;
            }
        }
    }
}
