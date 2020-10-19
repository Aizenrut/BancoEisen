using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace BancoEisen.Messaging.EmailProducer
{
    public class Startup
    {
        private IConnection currentConnection;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
