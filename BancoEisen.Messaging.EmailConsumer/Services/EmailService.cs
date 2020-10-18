using BancoEisen.Messaging.EmailConsumer.Services.Interfaces;
using BancoEisen.Messaging.EmailProducer.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BancoEisen.Messaging.EmailConsumer.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task EnviarEmailAsync(EmailMessage emailMessage)
        {
            var emailSection = configuration.GetSection("Email");

            using (var email = new MailMessage())
            {
                email.From = new MailAddress(emailSection["Address"], emailSection["DisplayName"]);
                email.To.Add(new MailAddress(emailMessage.Para));
                email.Subject = emailMessage.Assunto;
                email.Body = emailMessage.Conteudo;

                await EnviarPeloGmailAsync(email);
            }
        }

        private async Task EnviarPeloGmailAsync(MailMessage email)
        {
            var emailSection = configuration.GetSection("Email");

            using (var smtp = new SmtpClient())
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(emailSection["Address"], emailSection["Password"]);

                await smtp.SendMailAsync(email);
            }
        }
    }
}
