using BancoEisen.Data.Models;
using BancoEisen.Data.Services.Interfaces;
using BancoEisen.Models.Abstracoes;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BancoEisen.Data.Services
{
    public class OrdenacaoService<T> : IOrdenacaoService<T> where T : Entidade
    {
        public IQueryable<T> Ordenar(IQueryable<T> query, Ordem ordenacao)
        {
            if (ordenacao != null && !string.IsNullOrWhiteSpace(ordenacao.OrdenarPor))
            {
                query = query.OrderBy(ordenacao.OrdenarPor);
            }

            return query;
        }
    }
}
