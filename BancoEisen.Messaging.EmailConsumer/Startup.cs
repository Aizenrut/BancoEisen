using BancoEisen.Messaging.EmailConsumer.Services;
using BancoEisen.Messaging.EmailConsumer.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace BancoEisen.Messaging.EmailConsumer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(factory =>
            {
                return new ConnectionFactory() { HostName = "localhost" };
            });

            services.AddTransient(factory =>
            {
                var connectionfactory = factory.GetService<ConnectionFactory>();
                var connection = connectionfactory.CreateConnection();

                return connection.CreateModel();
            });

            services.AddTransient<IEmailService, EmailService>();
            services.AddHostedService<EmailConsumerService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
