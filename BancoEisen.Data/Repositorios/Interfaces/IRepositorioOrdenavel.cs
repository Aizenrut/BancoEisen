using BancoEisen.Data.Models.Ordens;
using BancoEisen.Models.Abstracoes;
using System.Linq;

namespace BancoEisen.Data.Repositorios.Interfaces
{
    public interface IRepositorioOrdenavel<T> : IRepositorioBase<T> where T : Entidade
    {
        IQueryable<T> Ordenar(IQueryable<T> query, Ordem ordem);
    }
}
