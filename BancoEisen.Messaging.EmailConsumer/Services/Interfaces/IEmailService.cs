using BancoEisen.Messaging.EmailProducer.Models;
using System.Threading.Tasks;

namespace BancoEisen.Messaging.EmailConsumer.Services.Interfaces
{
    public interface IEmailService
    {
        Task EnviarEmailAsync(EmailMessage emailMessage);
    }
}
