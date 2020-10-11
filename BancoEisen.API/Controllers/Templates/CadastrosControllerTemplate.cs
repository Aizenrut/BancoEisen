using BancoEisen.Controllers.Interfaces;
using BancoEisen.Models.Abstracoes;
using BancoEisen.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using BancoEisen.API.Models.Erros;
using System.Threading.Tasks;
using BancoEisen.Data.Models.Filtros.Interfaces;
using BancoEisen.API.Controllers.Interfaces;
using BancoEisen.Data.Models.Ordens;
using BancoEisen.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using BancoEisen.API.Models.Paginacoes;
using System.Linq;

namespace BancoEisen.API.Controllers.Templates
{
    public abstract class CadastrosControllerTemplate<TServico, TEntidade, TInformacoes, TFiltro> : ControllerBase, ICadastrosController<TEntidade, TInformacoes, TFiltro>
        where TServico : ICadastroController<TEntidade, TInformacoes, TFiltro>
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

        [HttpGet]
        public IActionResult Todos([FromQuery] TFiltro filtro,
                                   [FromQuery] Ordem ordem,
                                   [FromQuery] Paginacao paginacao)
        {
            var todos = servico.Todos(filtro, ordem).Select(x => x.ToResource())
                                                    .ToArray();

            return Ok(paginacaoService.GerarPagina(contextAccessor.HttpContext.GetRouteData().Values["controller"].ToString(), todos, paginacao));
        }

        [HttpGet("{id}")]
        public IActionResult Consultar(int id)
        {
            var entidade = servico.Consultar(id);

            if (entidade == null)
                return NotFound();

            return Ok(entidade.ToResource());
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(TInformacoes informacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var entidade = await servico.Cadastrar(informacoes);

            var uri = Url.Action(nameof(Consultar), new { id = entidade.Id });

            return Created(uri, entidade.ToResource());
        }

        [HttpPut]
        public async Task<IActionResult> Alterar(TEntidade entidade)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            await servico.Alterar(entidade);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            await servico.Remover(id);
            return NoContent();
        }
    }
}
