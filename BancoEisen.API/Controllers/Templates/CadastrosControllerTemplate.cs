using BancoEisen.Models.Abstracoes;
using BancoEisen.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using BancoEisen.API.Models;
using System.Threading.Tasks;
using BancoEisen.API.Controllers.Interfaces;
using BancoEisen.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using BancoEisen.Services;
using BancoEisen.Data.Models;
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
    public abstract class CadastrosControllerTemplate<TServico, TEntidade, TInformacoes, TFiltro> : ControllerBase, ICadastrosController<TEntidade, TInformacoes, TFiltro>
        where TServico : ICadastroService<TEntidade, TInformacoes, TFiltro>
        where TEntidade : Entidade
        where TInformacoes : struct
        where TFiltro : IFiltro<TEntidade>
    {
        protected readonly TServico servico;
        protected readonly IPaginacaoService paginacaoService;
        protected readonly IHttpContextAccessor contextAccessor;

        public CadastrosControllerTemplate(TServico servico, IPaginacaoService paginacaoService, IHttpContextAccessor contextAccessor)
        {
            this.servico = servico;
            this.paginacaoService = paginacaoService;
            this.contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Obtém todas as entidades existentes.
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
        /// Busca os dados de uma entidade.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Consultar(int id)
        {
            var entidade = servico.Consultar(id);

            if (entidade == null)
                return NotFound();

            return Ok(entidade.ToResource());
        }

        /// <summary>
        /// Realiza o cadastro da entidade.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Cadastrar(TInformacoes informacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var entidade = await servico.Cadastrar(informacoes);

            var uri = Url.Action(nameof(Consultar), new { id = entidade.Id });

            return Created(uri, entidade.ToResource());
        }

        /// <summary>
        /// Altera determinadas informações da entidade.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Alterar(TEntidade entidade)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var consulta = servico.Consultar(entidade.Id);

            if (consulta == null)
                return NotFound();

            await servico.Alterar(entidade);

            return Ok();
        }

        /// <summary>
        /// Realiza a exclusão da entidade que possui o id informado.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remover(int id)
        {
            var consulta = servico.Consultar(id);

            if (consulta == null)
                return NotFound();

            await servico.Remover(id);
            
            return NoContent();
        }
    }
}
