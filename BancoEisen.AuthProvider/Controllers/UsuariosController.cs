using BancoEisen.AuthProvider.Models;
using BancoEisen.AuthProvider.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BancoEisen.AuthProvider.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }

        [HttpGet("solicitarAlteracaoEmail")]
        public async Task<IActionResult> SolicitarRedefinicaoSenha([Required][FromQuery] string nomeUsuario,
                                                                   [Required][FromQuery] string novoEmail)
        {
            var usuario = await usuarioService.ObterPeloNomeAsync(nomeUsuario);

            if (usuario == null)
                return NotFound();

            await usuarioService.EnviarTokenAlteracaoEmailAsync(usuario, novoEmail);

            return Accepted();
        }

        [HttpGet("solicitarRedefinicaoSenha")]
        public async Task<IActionResult> SolicitarRedefinicaoSenha([Required][FromQuery] string nomeUsuario)
        {
            var usuario = await usuarioService.ObterPeloNomeAsync(nomeUsuario);

            if (usuario == null)
                return NotFound();

            await usuarioService.EnviarTokenRedefinicaoSenhaAsync(usuario);

            return Accepted();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(UsuarioInformacoes informacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await usuarioService.CadastrarAsync(informacoes);

            if (result != null)
            {
                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                var usuario = await usuarioService.ObterPeloNomeAsync(informacoes.NomeUsuario);

                await usuarioService.EnviarTokenConfirmacaoEmailAsync(usuario);
            }

            var loginUri = Url.Action("Autenticar", "Login", null, HttpContext.Request.Scheme);

            return Created(loginUri, null);
        }

        [HttpPatch]
        public async Task<IActionResult> ConfirmarEmail(ConfirmacaoInformacoes informacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var usuario = await usuarioService.ObterPeloNomeAsync(informacoes.NomeUsuario);

            if (usuario == null)
                return NotFound();

            var result = await usuarioService.ConfirmarEnderecoEmailAsync(usuario, informacoes.Token);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpPut("alterarEmail")]
        public async Task<IActionResult> AlterarEmail(AlteracaoEmailInformacoes informacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var usuario = await usuarioService.ObterPeloNomeAsync(informacoes.NomeUsuario);

            if (usuario == null)
                return NotFound();

            var resultAlteracao = await usuarioService.AlterarEmailAsync(usuario, informacoes.Token, informacoes.NovoEmail);

            if (!resultAlteracao.Succeeded)
                return BadRequest(resultAlteracao.Errors);

            var resultRemocao = await usuarioService.RemoverConfirmacaoEmailAsync(usuario);

            if (!resultRemocao.Succeeded)
                return BadRequest(resultRemocao.Errors);

            await usuarioService.EnviarTokenConfirmacaoEmailAsync(usuario);

            return Accepted();
        }

        [HttpPut("alterarSenha")]
        public async Task<IActionResult> AlterarSenha(AlteracaoSenhaInformacoes informacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var usuario = await usuarioService.ObterPeloNomeAsync(informacoes.NomeUsuario);

            if (usuario == null)
                return NotFound();

            var result = await usuarioService.AlterarSenhaAsync(usuario, informacoes.SenhaAtual, informacoes.NovaSenha);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpPut("alterarAutenticacaoDoisFatores")]
        public async Task<IActionResult> AlterarAutenticacaoDoisFatores(AlterarAutenticacaoDoisFatoresInformacoes informacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var usuario = await usuarioService.ObterPeloNomeAsync(informacoes.NomeUsuario);

            if (usuario == null)
                return NotFound();

            var result = await usuarioService.AlterarAutenticacaoDeDoisFatores(usuario, informacoes.UtilizarAutenticacaoDoisFatores);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpPut("redefinirSenha")]
        public async Task<IActionResult> RedefinirSenha(RedefinicaoSenhaInformacoes informacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var usuario = await usuarioService.ObterPeloNomeAsync(informacoes.NomeUsuario);

            if (usuario == null)
                return NotFound();

            var result = await usuarioService.RedefinirSenhaAsync(usuario, informacoes.Token, informacoes.NovaSenha);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpDelete("{nomeUsuario}")]
        public async Task<IActionResult> Remover(string nomeUsuario)
        {
            var usuario = await usuarioService.ObterPeloNomeAsync(nomeUsuario);

            var result = await usuarioService.RemoverAsync(usuario);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return NoContent();
        }
    }
}
