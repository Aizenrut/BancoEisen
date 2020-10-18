using BancoEisen.Data.Models;
using BancoEisen.Models.Abstracoes;
using System.Linq;

namespace BancoEisen.Data.Repositories
{
    public interface IRepositorioFiltravel<TEntidade, TFiltro> : IRepositorioBase<TEntidade>
        where TEntidade : Entidade
        where TFiltro : IFiltro<TEntidade>
    {
        IQueryable<TEntidade> Filtrar(IQueryable<TEntidade> query, TFiltro filtro);
    }
}
