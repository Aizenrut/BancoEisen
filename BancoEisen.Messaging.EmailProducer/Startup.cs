using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace BancoEisen.Messaging.EmailProducer
{
    public class Startup
    {
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
                var connectionfactory = factory.GetService<ConnectionFactory>();
                var connection = connectionfactory.CreateConnection();
                
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
