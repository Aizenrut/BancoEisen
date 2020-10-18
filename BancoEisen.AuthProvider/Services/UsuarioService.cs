using BancoEisen.AuthProvider.Models;
using BancoEisen.AuthProvider.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BancoEisen.AuthProvider.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UserManager<Usuario> userManager;
        private readonly IEmailService emailService;

        public UsuarioService(UserManager<Usuario> userManager,
                              IEmailService emailService)
        {
            this.userManager = userManager;
            this.emailService = emailService;
        }

        public async Task<Usuario> ObterPeloNomeAsync(string nomeUsuario)
        {
            return await userManager.FindByNameAsync(nomeUsuario);
        }

        public async Task<IdentityResult> CadastrarAsync(UsuarioInformacoes informacoes)
        {
            var userByEmail = await userManager.FindByEmailAsync(informacoes.Email);

            if (userByEmail != null)
                return null;

            var usuario = new Usuario(informacoes.NomeUsuario, informacoes.Email);

            return await userManager.CreateAsync(usuario, informacoes.Senha);
        }

        public async Task EnviarTokenConfirmacaoEmailAsync(Usuario usuario)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(usuario);

            await emailService.EnviarTokenConfirmacaoEmailAsync(usuario, token);
        }

        public async Task EnviarTokenRedefinicaoSenhaAsync(Usuario usuario)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(usuario);

            await emailService.EnviarTokenRedefinicaoSenhaAsync(usuario, token);
        }

        public async Task EnviarTokenAlteracaoEmailAsync(Usuario usuario, string novoEmail)
        {
            var token = await userManager.GenerateChangeEmailTokenAsync(usuario, novoEmail);

            await emailService.EnviarTokenAlteracaoEmailAsync(usuario, token);
        }

        public async Task<IdentityResult> ConfirmarEnderecoEmailAsync(Usuario usuario, string token)
        {
            return await userManager.ConfirmEmailAsync(usuario, token);
        }

        public async Task<IdentityResult> AlterarEmailAsync(Usuario usuario, string token, string novoEmail)
        {
            return await userManager.ChangeEmailAsync(usuario, novoEmail, token);
        }

        public async Task<IdentityResult> AlterarSenhaAsync(Usuario usuario, string senhaAtual, string novaSenha)
        {
            return await userManager.ChangePasswordAsync(usuario, senhaAtual, novaSenha);
        }

        public async Task<IdentityResult> RedefinirSenhaAsync(Usuario usuario, string token, string novaSenha)
        {
            return await userManager.ResetPasswordAsync(usuario, token, novaSenha);
        }

        public async Task<IdentityResult> AlterarAutenticacaoDeDoisFatores(Usuario usuario, bool utilizaAutenticacaoDeDoisFatores)
        {
            return await userManager.SetTwoFactorEnabledAsync(usuario, utilizaAutenticacaoDeDoisFatores);
        }

        public async Task<IdentityResult> RemoverConfirmacaoEmailAsync(Usuario usuario)
        {
            usuario.EmailConfirmed = false;
            return await userManager.UpdateAsync(usuario);
        }

        public async Task<IdentityResult> RemoverAsync(Usuario usuario)
        {
            return await userManager.DeleteAsync(usuario);
        }
    }
}
