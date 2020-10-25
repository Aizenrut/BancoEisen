using BancoEisen.API.Controllers.Templates;
using BancoEisen.API.Services;
using BancoEisen.Data.Models;
using BancoEisen.Models.Informacoes;
using BancoEisen.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BancoEisen.API.Controllers.Operacoes
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SaquesController : OperacoesControllerTamplate<ISaqueService, OperacaoUnariaInformacoes, SaqueFiltro>
    {
        public SaquesController(ISaqueService servico, IPaginacaoService paginacaoService, IHttpContextAccessor contextAccessor) 
            : base(servico, paginacaoService, contextAccessor)
        {
        }
    }
}
