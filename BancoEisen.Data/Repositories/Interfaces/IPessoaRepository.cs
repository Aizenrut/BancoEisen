using BancoEisen.Data.Models;
using BancoEisen.Models.Cadastros;

namespace BancoEisen.Data.Repositories
{
    public interface IPessoaRepository : IRepositorioFiltravel<Pessoa, PessoaFiltro>, IRepositorioOrdenavel<Pessoa>
    {
    }
}
