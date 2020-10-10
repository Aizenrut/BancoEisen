using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using BancoEisen.Controllers.Interfaces;
using BancoEisen.Controllers.Cadastros;
using BancoEisen.Controllers.Operacoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using BancoEisen.Models.Cadastros;
using BancoEisen.Data;
using BancoEisen.Models.Abstracoes;
using Microsoft.AspNetCore.Mvc;
using BancoEisen.API.Filters;

namespace BancoEisen.API
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            { 
                options.EnableEndpointRouting = false;
                options.Filters.Add<BadRequestFilter>(1);
                options.Filters.Add<ErrorResponseFilter>(0);
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddDbContext<BancoEisenContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("BancoEisen"));
            });

            services.AddTransient<IRepository<Conta>, Repository<Conta>>();
            services.AddTransient<IRepository<Pessoa>, Repository<Pessoa>>();
            services.AddTransient<IRepository<Usuario>, Repository<Usuario>>();
            services.AddTransient<IRepository<Operacao>, Repository<Operacao>>();

            services.AddTransient<IContaController, ContaController>();
            services.AddTransient<IPessoaController, PessoaController>();
            services.AddTransient<IUsuarioController, UsuarioController>();
            services.AddTransient<IDepositoController, DepositoController>();
            services.AddTransient<ISaqueController, SaqueController>();
            services.AddTransient<ITransferenciaController, TransferenciaController>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMvc();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
