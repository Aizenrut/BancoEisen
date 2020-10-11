using BancoEisen.Data.Models.Filtros;
using BancoEisen.Models.Cadastros;

namespace BancoEisen.Data.Repositorios.Interfaces
{
    public interface IPessoaRepositorio : IRepositorioFiltravel<Pessoa, PessoaFiltro>, IRepositorioOrdenavel<Pessoa>
    {
    }
}
