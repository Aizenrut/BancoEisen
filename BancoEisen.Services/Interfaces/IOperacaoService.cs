using BancoEisen.Data.Models;
using BancoEisen.Models.Abstracoes;
using System.Threading.Tasks;

namespace BancoEisen.Services
{
    public interface IOperacaoService<TInformacoes, TFiltro>
        where TInformacoes : struct
        where TFiltro : IFiltro<Operacao>
    {
        Operacao[] Todos(TFiltro filtro, Ordem ordem);
        Operacao Consultar(int id);
        Task<Operacao> Efetivar(TInformacoes informacoes);
    }
}