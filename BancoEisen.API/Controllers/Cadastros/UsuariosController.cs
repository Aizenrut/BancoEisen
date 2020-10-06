using BancoEisen.API.Controllers.Templates;
using BancoEisen.Controllers.Interfaces;
using BancoEisen.Models.Informacoes;
using BancoEisen.Models.Cadastros;
using Microsoft.AspNetCore.Mvc;

namespace BancoEisen.API.Controllers.Entidades
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : CadastrosControllerTemplate<IUsuarioController, UsuarioInformacoes, Usuario>
    {
        public UsuariosController(IUsuarioController servico) : base(servico)
        {
        }

        [HttpGet("{login}/estaDisponivel")]
        public IActionResult EstaDisponivel(string login)
        {
            return Ok(servico.EstaDisponivel(login));
        }
    }
}
