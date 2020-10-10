using BancoEisen.Models.Informacoes;
using BancoEisen.Models.Cadastros;

namespace BancoEisen.Controllers.Interfaces
{
    public interface IUsuarioController : ICadastroController<UsuarioInformacoes, Usuario>
    {
        bool EstaDisponivel(string usuarioLogin);
    }
}
