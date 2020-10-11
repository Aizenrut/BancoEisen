using BancoEisen.Controllers.Interfaces;
using BancoEisen.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using BancoEisen.API.Models.Erros;
using BancoEisen.API.Controllers.Interfaces;
using System.Threading.Tasks;
using BancoEisen.Data.Models.Filtros.Interfaces;
using BancoEisen.Models.Abstracoes;
using BancoEisen.Data.Models.Ordens;
using BancoEisen.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using BancoEisen.API.Models.Paginacoes;
using System.Linq;

namespace BancoEisen.API.Controllers.Templates
{
    public abstract class OperacoesControllerTamplate<TServico, TInformacoes, TFiltro> : ControllerBase, IOperacoesController<TInformacoes, TFiltro>
        where TServico : IOperacaoController<TInformacoes, TFiltro>
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
            var operacao = servico.Consultar(id);

            if (operacao == null)
                return NotFound();

            return Ok(operacao.ToResource());
        }

        [HttpPost]
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
