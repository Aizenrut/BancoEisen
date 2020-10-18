using BancoEisen.API.Controllers.Templates;
using BancoEisen.Models.Informacoes;
using BancoEisen.Models.Cadastros;
using Microsoft.AspNetCore.Mvc;
using BancoEisen.Data.Models;
using BancoEisen.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using BancoEisen.Services;

namespace BancoEisen.API.Controllers.Entidades
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : CadastrosControllerTemplate<IPessoaService, Pessoa, PessoaInformacoes, PessoaFiltro>
    {
        public PessoasController(IPessoaService servico, IPaginacaoService paginacaoService, IHttpContextAccessor contextAccessor) 
            : base(servico, paginacaoService, contextAccessor)
        {
        }
    }
}
