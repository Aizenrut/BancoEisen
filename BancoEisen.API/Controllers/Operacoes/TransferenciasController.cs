using BancoEisen.API.Controllers.Templates;
using BancoEisen.API.Services.Interfaces;
using BancoEisen.Controllers.Interfaces;
using BancoEisen.Data.Models.Filtros;
using BancoEisen.Models.Informacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BancoEisen.API.Controllers.Operacoes
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransferenciasController : OperacoesControllerTamplate<ITransferenciaController, OperacaoBinariaInformacoes, TransferenciaFiltro>
    {
        public TransferenciasController(ITransferenciaController servico, IPaginacaoService paginacaoService, IHttpContextAccessor contextAccessor) 
            : base(servico, paginacaoService, contextAccessor)
        {
        }
    }
}
