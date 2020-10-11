using BancoEisen.Data.Models.Filtros.Interfaces;
using BancoEisen.Models.Abstracoes;
using System.Linq;

namespace BancoEisen.Data.Services.Interfaces
{
    public interface IFiltragemService<TEntidade, TFiltro>
        where TEntidade : Entidade
        where TFiltro : IFiltro<TEntidade>
    {
        IQueryable<TEntidade> Filtrar(IQueryable<TEntidade> query, TFiltro filtro);
    }
}
