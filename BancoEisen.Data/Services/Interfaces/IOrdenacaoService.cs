using BancoEisen.Data.Models.Ordens;
using BancoEisen.Models.Abstracoes;
using System.Linq;

namespace BancoEisen.Data.Services.Interfaces
{
    public interface IOrdenacaoService<T> where T : Entidade
    {
        IQueryable<T> Ordenar(IQueryable<T> query, Ordem ordenacao);
    }
}
