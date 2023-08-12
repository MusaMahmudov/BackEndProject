using EduProject.ViewModels;

namespace EduProject.Services.Intefaces
{
    public interface IMailService
    {
        Task SendEMailAsync(MailRequest mailRequest);
    }
}
