using BancoEisen.API.Controllers.Templates;
using BancoEisen.API.Services;
using BancoEisen.Data.Models;
using BancoEisen.Models.Informacoes;
using BancoEisen.Services;
using Microsoft.AspNetCore.Http;

namespace BancoEisen.API.Controllers.Operacoes
{
    public class DepositosController : OperacoesControllerTamplate<IDepositoService, OperacaoUnariaInformacoes, DepositoFiltro>
    {
        public DepositosController(IDepositoService servico, IPaginacaoService paginacaoService, IHttpContextAccessor contextAccessor) 
            : base(servico, paginacaoService, contextAccessor)
        {
        }
    }
}
