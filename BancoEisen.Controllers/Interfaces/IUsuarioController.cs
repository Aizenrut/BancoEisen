using BancoEisen.Models.Informacoes;
using BancoEisen.Models.Cadastros;
using BancoEisen.Data.Models.Filtros;

namespace BancoEisen.Controllers.Interfaces
{
    public interface IUsuarioController : ICadastroController<Usuario, UsuarioInformacoes, UsuarioFiltro>
    {
        bool EstaDisponivel(string usuarioLogin);
    }
}
