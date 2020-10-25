using BancoEisen.Messaging.EmailConsumer.Services;
using BancoEisen.Messaging.EmailConsumer.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace BancoEisen.Messaging.EmailConsumer
{
    public class Startup
    {
        private IConfiguration configuration;
        private IConnection currentConnection;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(factory =>
            {
                var rabbitMqSection = configuration.GetSection("RabbitMQ");

                return new ConnectionFactory()
                {
                    HostName = rabbitMqSection["HostName"],
                    UserName = rabbitMqSection["UserName"],
                    Password = rabbitMqSection["Password"]
                };
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
