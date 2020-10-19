using BancoEisen.Messaging.EmailProducer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace BancoEisen.Messaging.EmailProducer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailProducerController : ControllerBase
    {
        private readonly IModel channel;
        private readonly ILogger<EmailProducerController> logger;

        public EmailProducerController(IModel channel, ILogger<EmailProducerController> logger)
        {
            this.channel = channel;
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult Produce(EmailMessage email)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var message = JsonConvert.SerializeObject(email);
            var bytes = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: "emails",
                                 basicProperties: null,
                                 body: bytes);

            channel.Dispose();

            logger.LogInformation($"Enfileirado \"{email.Assunto}\" para {email.Para}");

            return Ok();
        }
    }
}
