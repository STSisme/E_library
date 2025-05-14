namespace E_Library.Entities
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

}
