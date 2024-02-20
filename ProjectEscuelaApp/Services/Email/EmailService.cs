using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using WebEscuelaProject.Models;

namespace WebEscuelaProject.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailSettings:EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();

            smtp.Connect(_config.GetSection("EmailSettings:EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailSettings:EmailUsername").Value, _config.GetSection("EmailSettings:EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
