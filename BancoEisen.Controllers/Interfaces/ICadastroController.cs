using BancoEisen.Data.Models.Filtros.Interfaces;
using BancoEisen.Data.Models.Ordens;
using BancoEisen.Models.Abstracoes;
using System.Threading.Tasks;

namespace BancoEisen.Controllers.Interfaces
{
    public interface ICadastroController<TEntidade, TInformacoes, TFiltro>
        where TEntidade : Entidade
        where TInformacoes : struct
        where TFiltro : IFiltro<TEntidade>
    {
        TEntidade[] Todos(TFiltro filtro, Ordem ordem);
        TEntidade Consultar(int id);
        Task<TEntidade> Cadastrar(TInformacoes informacoes);
        Task Alterar(TEntidade entidade);
        Task Remover(int id);
    }
}
