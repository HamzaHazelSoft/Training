using UserManagementSystem.Models;

namespace UserManagementSystem.Services.Mail
{
    public interface IMailService
    {
        public Task SendMailAsync(string recipient, string subject,string body);
        public Task SendConfirmationEmailAsync(User user);

    }
}
