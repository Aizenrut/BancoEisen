using BancoEisen.Data.Models;
using BancoEisen.Models.Abstracoes;
using System.Linq;

namespace BancoEisen.Data.Repositories
{
    public interface IRepositorioOrdenavel<T> : IRepositorioBase<T> where T : Entidade
    {
        IQueryable<T> Ordenar(IQueryable<T> query, Ordem ordem);
    }
}
