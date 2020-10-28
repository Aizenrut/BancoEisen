using BancoEisen.AuthProvider.Clients;
using BancoEisen.AuthProvider.Data;
using BancoEisen.AuthProvider.Filters;
using BancoEisen.AuthProvider.Models;
using BancoEisen.AuthProvider.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;

namespace BancoEisen.AuthProvider
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
                options.Filters.Add<ErrorResponseFilter>();
            }).AddXmlSerializerFormatters();

            services.AddApiVersioning();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddDbContext<BancoEisenAuthenticationContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("BancoEisen"));
            });

            services.AddIdentity<Usuario, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<BancoEisenAuthenticationContext>()
              .AddDefaultTokenProviders()
              .AddTokenProvider<EmailTokenProvider<Usuario>>("Email");

            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();

            services.AddTransient<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddHttpClient<EmailProducerClient>(options =>
            {
                options.BaseAddress = new Uri("http://eisen-producer:80/api/");
            });

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IUsuarioService, UsuarioService>();

            services.TryAddTransient<JwtSecurityTokenHandler>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Title = "Provedor de Autenticação",
                    Version = "1.0",
                    Description = "API utilizada para realizar as operações referentes ao usuário e autenticação."
                });
                
                var arquivo = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var caminho = Path.Combine(AppContext.BaseDirectory, arquivo);
                options.IncludeXmlComments(caminho);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/Swagger/v1.0/swagger.json", "Versão 1.0");
                options.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
