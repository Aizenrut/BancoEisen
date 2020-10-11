using BancoEisen.Data.Models.Filtros;
using BancoEisen.Models.Cadastros;

namespace BancoEisen.Data.Repositorios.Interfaces
{
    public interface IUsuarioRepositorio : IRepositorioFiltravel<Usuario, UsuarioFiltro>, IRepositorioOrdenavel<Usuario>
    {
        bool EstaDisponivel(string login);
    }
}
