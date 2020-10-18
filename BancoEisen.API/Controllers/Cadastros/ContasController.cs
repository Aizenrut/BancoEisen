using BancoEisen.API.Controllers.Templates;
using BancoEisen.Controllers.Interfaces;
using BancoEisen.Models.Informacoes;
using BancoEisen.Models.Cadastros;
using Microsoft.AspNetCore.Mvc;
using BancoEisen.Data.Models.Filtros;
using BancoEisen.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace BancoEisen.API.Controllers.Entidades
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContasController : CadastrosControllerTemplate<IContaController, Conta, ContaInformacoes, ContaFiltro>
    {
        public ContasController(IContaController servico, IPaginacaoService paginacaoService, IHttpContextAccessor contextAccessor) 
            : base(servico, paginacaoService, contextAccessor)
        {
        }
    }
}
