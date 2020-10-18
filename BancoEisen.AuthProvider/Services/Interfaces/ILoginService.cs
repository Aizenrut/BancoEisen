using BancoEisen.AuthProvider.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BancoEisen.AuthProvider.Services
{
    public interface ILoginService
    {
        Task<SignInResult> AutenticarAsync(Credenciais credenciais);
        Task<SignInResult> AutenticarDoisFatoresAsync(CredenciaisDoisFatores credenciais);
        Task EnviarTokenAutenticacaoDoisFatoresAsync(string nomeUsuario);
        Task<bool> EhSenhaCorretaAsync(string nomeUsuario, string senha);
        string GerarToken(string nomeUsuario);
    }
}
