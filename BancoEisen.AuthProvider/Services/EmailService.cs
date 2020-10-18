using BancoEisen.AuthProvider.Clients;
using BancoEisen.AuthProvider.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BancoEisen.AuthProvider.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailProducerClient client;
        private readonly IUrlHelper urlHelper;

        public EmailService(EmailProducerClient client,
                            IUrlHelper urlHelper)
        {
            this.client = client;
            this.urlHelper = urlHelper;
        }

        public async Task EnviarTokenConfirmacaoEmailAsync(Usuario usuario, string token)
        {
            await client.EnfileirarEmailAsync(usuario.Email, "Confirmação de e-mail", GerarMensagemConfirmacaoEmail(usuario, token));
        }
        
        public async Task EnviarTokenAlteracaoEmailAsync(Usuario usuario, string token)
        {
            await client.EnfileirarEmailAsync(usuario.Email, "Alteração de e-mail", GerarMensagemAlteracaoEmail(usuario, token));
        }

        public async Task EnviarTokenRedefinicaoSenhaAsync(Usuario usuario, string token)
        {
            await client.EnfileirarEmailAsync(usuario.Email, "Redefinição de senha", GerarMensagemRedefinicaoSenha(usuario, token));
        }

        public async Task EnviarTokenAutenticacaoDoisFatoresAsync(Usuario usuario, string token)
        {
            await client.EnfileirarEmailAsync(usuario.Email, "Autenticação de dois fatores", GerarMensagemAutenticacaoDoisFatores(usuario, token));
        }

        private string GerarMensagemConfirmacaoEmail(Usuario usuario, string token)
        {
            var actionConfirmarEmail = urlHelper.Action("ConfirmarEmail", "Usuarios", null, urlHelper.ActionContext.HttpContext.Request.Scheme);
            return $"Olá {usuario.UserName},\n\nEnvie uma requisição PATCH para {actionConfirmarEmail} com o token abaixo para confirmar o seu e-mail.\n\n{token}";
        }

        private string GerarMensagemAlteracaoEmail(Usuario usuario, string token)
        {
            var actioinAlterarEmail = urlHelper.Action("AlterarEmail", "Usuarios", null, urlHelper.ActionContext.HttpContext.Request.Scheme);
            return $"Olá {usuario.UserName},\n\nEnvie uma requisição PUT para {actioinAlterarEmail} com o token abaixo para concluir a alteração.\n\n{token}";
        }

        private string GerarMensagemRedefinicaoSenha(Usuario usuario, string token)
        {
            var actionRedefinirSenha = urlHelper.Action("RedefinirSenha", "Usuarios", null, urlHelper.ActionContext.HttpContext.Request.Scheme);
            return $"Olá {usuario.UserName},\n\nEnvie uma requisição PUT para {actionRedefinirSenha} com o token abaixo para redefinir a sua senha.\n\n{token}";
        }

        private string GerarMensagemAutenticacaoDoisFatores(Usuario usuario, string token)
        {
            var actionAutenticarDoisFatores = urlHelper.Action("AutenticarDoisFatores", "Login", null, urlHelper.ActionContext.HttpContext.Request.Scheme);
            return $"Olá {usuario.UserName},\n\nEnvie uma requisição PATCH para {actionAutenticarDoisFatores} com o token abaixo para concluir a autenticação.\n\n{token}";
        }
    }
}
