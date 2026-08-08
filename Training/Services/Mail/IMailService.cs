namespace Training.Services.Mail
{
    public interface IMailService
    {
        public Task SendMailAsync(string recipient, string subject,string body);

    }
}
