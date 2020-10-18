using BancoEisen.AuthProvider.Clients;
using BancoEisen.AuthProvider.Data;
using BancoEisen.AuthProvider.Filters;
using BancoEisen.AuthProvider.Models;
using BancoEisen.AuthProvider.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.IdentityModel.Tokens.Jwt;

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

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddDbContext<BancoEisenAuthenticationContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("BancoEisen.Authentication"));
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
                options.BaseAddress = new Uri("http://localhost:7000/api/");
            });

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IUsuarioService, UsuarioService>();

            services.TryAddTransient<JwtSecurityTokenHandler>();
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
