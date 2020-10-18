using BancoEisen.Data.Models;
using BancoEisen.Models.Cadastros;

namespace BancoEisen.Data.Repositories
{
    public interface IContaRepository : IRepositorioFiltravel<Conta, ContaFiltro>, IRepositorioOrdenavel<Conta>
    {
    }
}
