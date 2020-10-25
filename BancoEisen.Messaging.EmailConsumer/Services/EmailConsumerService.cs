using BancoEisen.Messaging.EmailConsumer.Services.Interfaces;
using BancoEisen.Messaging.EmailProducer.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BancoEisen.Messaging.EmailConsumer.Services
{
    public class EmailConsumerService : BackgroundService
    {
        private readonly IModel channel;
        private readonly IEmailService emailService;
        private readonly ILogger<EmailConsumerService> logger;

        public EmailConsumerService(IModel channel,
                                    IEmailService emailService,
                                    ILogger<EmailConsumerService> logger)
        {
            this.channel = channel;
            this.emailService = emailService;
            this.logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += Consume;

            channel.BasicConsume(queue: "emails",
                                 autoAck: false,
                                 consumer: consumer);

            return Task.CompletedTask;
        }

        public async void Consume(object model, BasicDeliverEventArgs eventArgs)
        {
            await new TaskFactory().StartNew(() => ConsumeAsync(model, eventArgs));
        }

        private async Task ConsumeAsync(object model, BasicDeliverEventArgs eventArgs)
        {
            try
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var emailMessage = JsonConvert.DeserializeObject<EmailMessage>(message);

                await emailService.EnviarEmailAsync(emailMessage);

                channel.BasicAck(eventArgs.DeliveryTag, false);

                logger.LogInformation($"E-mail \"{emailMessage.Assunto}\" enviado para {emailMessage.Para}.");
            }
            catch (Exception e)
            {
                channel.BasicNack(eventArgs.DeliveryTag, false, true);

                logger.LogError($"Erro: {e.Message}", e);
            }
        }

        public override void Dispose()
        {
            channel.Dispose();
            base.Dispose();
        }
    }
}
