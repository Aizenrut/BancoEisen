using BancoEisen.Data.Models.Filtros.Interfaces;
using BancoEisen.Data.Models.Ordens;
using BancoEisen.Models.Abstracoes;
using System.Threading.Tasks;

namespace BancoEisen.Controllers.Interfaces
{
    public interface IOperacaoController<TInformacoes, TFiltro>
        where TInformacoes : struct
        where TFiltro : IFiltro<Operacao>
    {
        Operacao[] Todos(TFiltro filtro, Ordem ordem);
        Operacao Consultar(int id);
        Task<Operacao> Efetivar(TInformacoes informacoes);
    }
}