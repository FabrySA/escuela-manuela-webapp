using WebEscuelaProject.Models;

namespace WebEscuelaProject.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
    }
}
