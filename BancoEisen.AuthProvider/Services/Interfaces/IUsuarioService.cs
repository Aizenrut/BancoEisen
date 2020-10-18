using BancoEisen.AuthProvider.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BancoEisen.AuthProvider.Services
{
    public interface IUsuarioService
    {
        Task<Usuario> ObterPeloNomeAsync(string nomeUsuario);
        Task<IdentityResult> CadastrarAsync(UsuarioInformacoes informacoes);
        Task EnviarTokenConfirmacaoEmailAsync(Usuario usuario);
        Task EnviarTokenRedefinicaoSenhaAsync(Usuario usuario);
        Task EnviarTokenAlteracaoEmailAsync(Usuario usuario, string novoEmail);
        Task<IdentityResult> ConfirmarEnderecoEmailAsync(Usuario usuario, string token);
        Task<IdentityResult> AlterarEmailAsync(Usuario usuario, string token, string novoEmail);
        Task<IdentityResult> AlterarSenhaAsync(Usuario usuario, string senhaAtual, string novaSenha);
        Task<IdentityResult> RedefinirSenhaAsync(Usuario usuario, string token, string novaSenha);
        Task<IdentityResult> AlterarAutenticacaoDeDoisFatores(Usuario usuario, bool utilizaAutenticacaoDeDoisFatores);
        Task<IdentityResult> RemoverConfirmacaoEmailAsync(Usuario usuario);
        Task<IdentityResult> RemoverAsync(Usuario usuario);
    }
}
