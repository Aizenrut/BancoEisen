using BancoEisen.Controllers.Interfaces;
using BancoEisen.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;

namespace BancoEisen.API.Controllers.Templates
{
    public abstract class OperacoesControllerTamplate<TServico, TInformacoes> : ControllerBase
        where TServico : IOperacaoController<TInformacoes>
        where TInformacoes : struct
    {
        private readonly TServico servico;

        public OperacoesControllerTamplate(TServico servico)
        {
            this.servico = servico;
        }

        [HttpGet]
        public IActionResult Todos()
        {
            return Ok(servico.Todos().Select(operacao => operacao.ToResource()));
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
        public IActionResult Efetivar(TInformacoes operacaoInformacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var operacao = servico.Efetivar(operacaoInformacoes);

                var uri = Url.Action(nameof(Consultar), new { id = operacao.Id });

                return Created(uri, operacao.ToResource());
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
