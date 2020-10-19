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
        private IConnection currentConnection;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(factory =>
            {
                return new ConnectionFactory() { HostName = "localhost" };
            });

            services.AddTransient(factory =>
            {
                if (currentConnection == null || !currentConnection.IsOpen)
                {
                    var connectionfactory = factory.GetService<ConnectionFactory>();
                    currentConnection = connectionfactory.CreateConnection();
                }

                return currentConnection;
            });

            services.AddTransient(factory =>
            {
                var connection = factory.GetService<IConnection>();
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
