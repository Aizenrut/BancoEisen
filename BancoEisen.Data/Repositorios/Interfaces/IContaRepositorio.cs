using BancoEisen.Data.Models.Filtros;
using BancoEisen.Models.Cadastros;

namespace BancoEisen.Data.Repositorios.Interfaces
{
    public interface IContaRepositorio : IRepositorioFiltravel<Conta, ContaFiltro>, IRepositorioOrdenavel<Conta>
    {
    }
}
