using BancoEisen.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using BancoEisen.API.Models;
using BancoEisen.API.Controllers.Interfaces;
using System.Threading.Tasks;
using BancoEisen.Models.Abstracoes;
using BancoEisen.Data.Models;
using BancoEisen.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using BancoEisen.Services;
using Microsoft.AspNetCore.Authorization;

namespace BancoEisen.API.Controllers.Templates
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1.0")]
    [Consumes("application/json", "text/json")]
    [Produces("application/json", "text/json", "application/xml", "text/xml")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public abstract class OperacoesControllerTamplate<TServico, TInformacoes, TFiltro> : ControllerBase, IOperacoesController<TInformacoes, TFiltro>
        where TServico : IOperacaoService<TInformacoes, TFiltro>
        where TInformacoes : struct
        where TFiltro : IFiltro<Operacao>
    {
        protected readonly TServico servico;
        protected readonly IPaginacaoService paginacaoService;
        protected readonly IHttpContextAccessor contextAccessor;

        public OperacoesControllerTamplate(TServico servico, IPaginacaoService paginacaoService, IHttpContextAccessor contextAccessor)
        {
            this.servico = servico;
            this.paginacaoService = paginacaoService;
            this.contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Obtém todas as operações existentes.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Pagina))]
        public IActionResult Todos([FromQuery] TFiltro filtro,
                                   [FromQuery] Ordem ordem,
                                   [FromQuery] Paginacao paginacao)
        {
            var todos = servico.Todos(filtro, ordem).Select(x => x.ToResource())
                                                    .ToArray();

            return Ok(paginacaoService.GerarPagina(contextAccessor.HttpContext.GetRouteData().Values["controller"].ToString(), todos, paginacao));
        }

        /// <summary>
        /// Busca os dados de uma operação.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Consultar(int id)
        {
            var operacao = servico.Consultar(id);

            if (operacao == null)
                return NotFound();

            return Ok(operacao.ToResource());
        }

        /// <summary>
        /// Efetiva a operação.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Efetivar(TInformacoes operacaoInformacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var operacao = await servico.Efetivar(operacaoInformacoes);

            var uri = Url.Action(nameof(Consultar), new { id = operacao.Id });

            return Created(uri, operacao.ToResource());
        }
    }
}
