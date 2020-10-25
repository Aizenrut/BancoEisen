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
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ContasController : CadastrosControllerTemplate<IContaService, Conta, ContaInformacoes, ContaFiltro>
    {
        public ContasController(IContaService servico, IPaginacaoService paginacaoService, IHttpContextAccessor contextAccessor) 
            : base(servico, paginacaoService, contextAccessor)
        {
        }
    }
}
