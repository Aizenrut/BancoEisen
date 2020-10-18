using BancoEisen.AuthProvider.Models;
using System.Threading.Tasks;

namespace BancoEisen.AuthProvider.Services
{
    public interface IEmailService
    {
        Task EnviarTokenConfirmacaoEmailAsync(Usuario usuario, string token);
        Task EnviarTokenAlteracaoEmailAsync(Usuario usuario, string token);
        Task EnviarTokenRedefinicaoSenhaAsync(Usuario usuario, string token);
        Task EnviarTokenAutenticacaoDoisFatoresAsync(Usuario usuario, string token);
    }
}
