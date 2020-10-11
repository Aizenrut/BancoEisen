using BancoEisen.API.Controllers.Templates;
using BancoEisen.Controllers.Interfaces;
using BancoEisen.Models.Informacoes;
using BancoEisen.Models.Cadastros;
using Microsoft.AspNetCore.Mvc;
using BancoEisen.API.Services.Interfaces;
using BancoEisen.Data.Models.Filtros;
using Microsoft.AspNetCore.Http;

namespace BancoEisen.API.Controllers.Entidades
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : CadastrosControllerTemplate<IUsuarioController, Usuario, UsuarioInformacoes, UsuarioFiltro>
    {
        public UsuariosController(IUsuarioController servico, IPaginacaoService paginacaoService, IHttpContextAccessor contextAccessor) 
            : base(servico, paginacaoService, contextAccessor)
        {
        }

        [HttpGet("{login}/estaDisponivel")]
        public IActionResult EstaDisponivel(string login)
        {
            return Ok(servico.EstaDisponivel(login));
        }
    }
}
