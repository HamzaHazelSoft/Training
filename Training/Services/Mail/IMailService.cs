namespace UserManagementSystem.Services.Mail
{
    public interface IMailService
    {
        public Task SendMailAsync(string recipient, string subject,string body);
        public Task SendConfirmationEmailAsync(string recipient,string userId,string token);

    }
}
