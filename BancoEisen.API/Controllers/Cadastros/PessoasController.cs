using BancoEisen.API.Controllers.Templates;
using BancoEisen.Controllers.Interfaces;
using BancoEisen.Models.Informacoes;
using BancoEisen.Models.Cadastros;
using Microsoft.AspNetCore.Mvc;

namespace BancoEisen.API.Controllers.Entidades
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : CadastrosControllerTemplate<IPessoaController, PessoaInformacoes, Pessoa>
    {
        public PessoasController(IPessoaController pessoaServico) : base(pessoaServico)
        {
        }
    }
}
