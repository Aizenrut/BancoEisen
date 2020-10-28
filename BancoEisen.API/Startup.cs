using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using BancoEisen.API.Filters;
using BancoEisen.Data.Repositories;
using BancoEisen.Data.Contexts;
using BancoEisen.API.Services;
using BancoEisen.Models.Cadastros;
using BancoEisen.Data.Services.Interfaces;
using BancoEisen.Data.Services;
using BancoEisen.Data.Models;
using BancoEisen.Models.Abstracoes;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using BancoEisen.Services;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
            }).AddXmlSerializerFormatters();

            services.AddApiVersioning();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddDbContext<BancoEisenContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("BancoEisen"));
            });

            var schemesSection = configuration.GetSection("Schemes");
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = schemesSection["Authentication"];
                options.DefaultChallengeScheme = schemesSection["Challenge"];
            }).AddJwtBearer(schemesSection["Authentication"], options =>
            {
                var keysSection = configuration.GetSection("Keys");
                var claimsSection = configuration.GetSection("Claims");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = claimsSection["Iss"],
                    ValidAudience = claimsSection["Aud"],
                    ClockSkew = TimeSpan.FromMinutes(5),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keysSection["JwtBearer"]))
                };
            });

            services.AddHttpContextAccessor();

            services.AddTransient<IContaRepository, ContaRepository>();
            services.AddTransient<IPessoaRepository, PessoaRepository>();
            services.AddTransient<IOperacaoRepository, OperacaoRepository>();

            services.AddTransient<IContaService, ContaService>();
            services.AddTransient<IPessoaService, PessoaService>();
            services.AddTransient<IDepositoService, DepositoService>();
            services.AddTransient<ISaqueService, SaqueService>();
            services.AddTransient<ITransferenciaService, TransferenciaService>();
            
            services.AddTransient<IFiltragemService<Conta, ContaFiltro>, FiltragemService<Conta, ContaFiltro>>();
            services.AddTransient<IFiltragemService<Pessoa, PessoaFiltro>, FiltragemService<Pessoa, PessoaFiltro>>();
            services.AddTransient<IFiltragemService<Operacao, OperacaoFiltro>, FiltragemService<Operacao, OperacaoFiltro>>();
            services.AddTransient<IFiltragemService<Operacao, DepositoFiltro>, FiltragemService<Operacao, DepositoFiltro>>();
            services.AddTransient<IFiltragemService<Operacao, SaqueFiltro>, FiltragemService<Operacao, SaqueFiltro>>();
            services.AddTransient<IFiltragemService<Operacao, TransferenciaFiltro>, FiltragemService<Operacao, TransferenciaFiltro>>();
            
            services.AddTransient<IOrdenacaoService<Conta>, OrdenacaoService<Conta>>();
            services.AddTransient<IOrdenacaoService<Pessoa>, OrdenacaoService<Pessoa>>();
            services.AddTransient<IOrdenacaoService<Operacao>, OrdenacaoService<Operacao>>();

            services.AddTransient<IPaginacaoService, PaginacaoService>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Title = "API do Banco Eisen",
                    Version = "1.0",
                    Description = "API utilizada para realizar operações do banco."
                });

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Autenticação utilizando o esquema Bearer. Ex.: Bearer {token}",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT"
                });

                options.OperationFilter<AddJwtBearerAuthOperationFilter>();

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

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();
        }
    }
}
