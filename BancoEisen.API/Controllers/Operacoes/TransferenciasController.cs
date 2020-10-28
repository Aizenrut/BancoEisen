using BancoEisen.API.Controllers.Templates;
using BancoEisen.API.Services;
using BancoEisen.Data.Models;
using BancoEisen.Models.Informacoes;
using BancoEisen.Services;
using Microsoft.AspNetCore.Http;

namespace BancoEisen.API.Controllers.Operacoes
{
    public class TransferenciasController : OperacoesControllerTamplate<ITransferenciaService, OperacaoBinariaInformacoes, TransferenciaFiltro>
    {
        public TransferenciasController(ITransferenciaService servico, IPaginacaoService paginacaoService, IHttpContextAccessor contextAccessor) 
            : base(servico, paginacaoService, contextAccessor)
        {
        }
    }
}
