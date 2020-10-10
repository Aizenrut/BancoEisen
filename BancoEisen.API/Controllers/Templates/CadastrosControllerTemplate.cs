using BancoEisen.Controllers.Interfaces;
using BancoEisen.Models.Abstracoes;
using BancoEisen.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using BancoEisen.API.Models.Erros;

namespace BancoEisen.API.Controllers.Templates
{
    public abstract class CadastrosControllerTemplate<TServico, TInformacoes, TEntidade> : ControllerBase
        where TServico : ICadastroController<TInformacoes, TEntidade>
        where TInformacoes : struct
        where TEntidade : Entidade
    {
        protected readonly TServico servico;

        public CadastrosControllerTemplate(TServico servico)
        {
            this.servico = servico;
        }

        [HttpGet]
        public IActionResult Todos()
        {
            return Ok(servico.Todos().Select(entidade => entidade.ToResource()));
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
        public IActionResult Cadastrar(TInformacoes informacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var entidade = servico.Cadastrar(informacoes);

            var uri = Url.Action(nameof(Consultar), new { id = entidade.Id });

            return Created(uri, entidade.ToResource());
        }

        [HttpPut]
        public IActionResult Alterar(TEntidade entidade)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            servico.Alterar(entidade);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Remover(int id)
        {
            servico.Remover(id);
            return NoContent();
        }
    }
}
