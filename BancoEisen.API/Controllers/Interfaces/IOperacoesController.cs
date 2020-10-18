using BancoEisen.API.Models;
using BancoEisen.Data.Models;
using BancoEisen.Models.Abstracoes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BancoEisen.API.Controllers.Interfaces
{
    public interface IOperacoesController<TInformacoes, TFiltro>
        where TInformacoes : struct
        where TFiltro : IFiltro<Operacao>
    {
        IActionResult Todos(TFiltro filtro, Ordem ordem, Paginacao paginacao);
        IActionResult Consultar(int id);
        Task<IActionResult> Efetivar(TInformacoes informacoes);
    }
}
