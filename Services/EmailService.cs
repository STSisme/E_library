using E_Library.Entities;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace E_Library.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var host = _config["Smtp:Host"];
            var port = int.Parse(_config["Smtp:Port"]);
            var user = _config["Smtp:User"];
            var pass = _config["Smtp:Pass"];

            if (string.IsNullOrEmpty(host))
                throw new Exception("SMTP Host is not configured");

            var smtp = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                Credentials = new NetworkCredential(user, pass)
            };

            var mail = new MailMessage(user, to, subject, body)
            {
                IsBodyHtml = false
            };

            await smtp.SendMailAsync(mail);
        }

    }
}
