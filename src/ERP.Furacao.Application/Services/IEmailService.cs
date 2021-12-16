using System.Threading.Tasks;
using ERP.Furacao.Application.DTOs.Mail;

namespace ERP.Furacao.Application.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
