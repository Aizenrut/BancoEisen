using BancoEisen.AuthProvider.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BancoEisen.AuthProvider.Services
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<Usuario> userManager;
        private readonly SignInManager<Usuario> signInManager;
        private readonly JwtSecurityTokenHandler tokenHandler;
        private readonly IEmailService emailService;

        public LoginService(IConfiguration configuration,
                            UserManager<Usuario> userManager,
                            SignInManager<Usuario> signInManager,
                            JwtSecurityTokenHandler tokenHandler,
                            IEmailService emailService)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenHandler = tokenHandler;
            this.emailService = emailService;
        }

        public async Task<SignInResult> AutenticarAsync(Credenciais credenciais)
        {
            return await signInManager.PasswordSignInAsync(credenciais.NomeUsuario, credenciais.Senha, false, true);
        }

        public async Task<SignInResult> AutenticarDoisFatoresAsync(CredenciaisDoisFatores credenciais)
        {
            return await signInManager.TwoFactorSignInAsync("Email", credenciais.Token, false, false);
        }

        public async Task EnviarTokenAutenticacaoDoisFatoresAsync(string nomeUsuario)
        {
            var usuario = await userManager.FindByNameAsync(nomeUsuario);

            var token = await userManager.GenerateTwoFactorTokenAsync(usuario, "Email");

            await emailService.EnviarTokenAutenticacaoDoisFatoresAsync(usuario, token);
        }

        public async Task<bool> EhSenhaCorretaAsync(string nomeUsuario, string senha)
        {
            var usuario = await userManager.FindByNameAsync(nomeUsuario);

            return await userManager.CheckPasswordAsync(usuario, senha);
        }

        public string GerarToken(string nomeUsuario)
        {
            var claimsSection = configuration.GetSection("Claims");
            var keysSection = configuration.GetSection("Keys");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nomeUsuario),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keysSection["JwtBearer"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: claimsSection["Iss"],
                                             audience: claimsSection["Aud"],
                                             claims,
                                             notBefore: DateTime.Now,
                                             expires: DateTime.Now.AddMinutes(30),
                                             signingCredentials);

            return tokenHandler.WriteToken(token);
        }
    }
}
