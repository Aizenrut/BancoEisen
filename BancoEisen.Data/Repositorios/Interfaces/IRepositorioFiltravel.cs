using BancoEisen.Data.Models.Filtros.Interfaces;
using BancoEisen.Models.Abstracoes;
using System.Linq;

namespace BancoEisen.Data.Repositorios
{
    public interface IRepositorioFiltravel<TEntidade, TFiltro> : IRepositorioBase<TEntidade>
        where TEntidade : Entidade
        where TFiltro : IFiltro<TEntidade>
    {
        IQueryable<TEntidade> Filtrar(IQueryable<TEntidade> query, TFiltro filtro);
    }
}
