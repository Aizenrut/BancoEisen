using BancoEisen.AuthProvider.Models;
using BancoEisen.AuthProvider.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BancoEisen.AuthProvider.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1.0")]
    [Consumes("application/json", "text/json")]
    [Produces("application/json", "text/json", "application/xml", "text/xml")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }

        /// <summary>
        /// Solicita o envio de e-mail contendo o token para redefinição de e-mail.
        /// </summary>
        [HttpGet("solicitarAlteracaoEmail")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SolicitarRedefinicaoEmail([Required][FromQuery] string nomeUsuario,
                                                                   [Required][FromQuery] string novoEmail)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var usuario = await usuarioService.ObterPeloNomeAsync(nomeUsuario);

            if (usuario == null)
                return NotFound();

            await usuarioService.EnviarTokenAlteracaoEmailAsync(usuario, novoEmail);

            return Accepted();
        }

        /// <summary>
        /// Solicita o envio de e-mail contendo o token para redefinição de senha.
        /// </summary>
        [HttpGet("solicitarRedefinicaoSenha")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> SolicitarRedefinicaoSenha([Required][FromQuery] string nomeUsuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var usuario = await usuarioService.ObterPeloNomeAsync(nomeUsuario);

            if (usuario == null)
                return NotFound();

            await usuarioService.EnviarTokenRedefinicaoSenhaAsync(usuario);

            return Accepted();
        }

        /// <summary>
        /// Realiza a criação do usuário.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Cadastrar(UsuarioInformacoes informacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var result = await usuarioService.CadastrarAsync(informacoes);

            if (result != null)
            {
                if (!result.Succeeded)
                    return BadRequest(ErrorResponse.From(result.Errors));

                var usuario = await usuarioService.ObterPeloNomeAsync(informacoes.NomeUsuario);

                await usuarioService.EnviarTokenConfirmacaoEmailAsync(usuario);
            }

            var loginUri = Url.Action("Autenticar", "Login", null, HttpContext.Request.Scheme);

            return Created(loginUri, null);
        }

        /// <summary>
        /// Realiza a confirmação de e-mail.
        /// </summary>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmarEmail(ConfirmacaoInformacoes informacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var usuario = await usuarioService.ObterPeloNomeAsync(informacoes.NomeUsuario);

            if (usuario == null)
                return NotFound();

            var result = await usuarioService.ConfirmarEnderecoEmailAsync(usuario, informacoes.Token);

            if (!result.Succeeded)
                return BadRequest(ErrorResponse.From(result.Errors));

            return Ok();
        }

        /// <summary>
        /// Realiza a alteração de e-mail.
        /// </summary>
        [HttpPut("alterarEmail")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AlterarEmail(AlteracaoEmailInformacoes informacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var usuario = await usuarioService.ObterPeloNomeAsync(informacoes.NomeUsuario);

            if (usuario == null)
                return NotFound();

            var resultAlteracao = await usuarioService.AlterarEmailAsync(usuario, informacoes.Token, informacoes.NovoEmail);

            if (!resultAlteracao.Succeeded)
                return BadRequest(ErrorResponse.From(resultAlteracao.Errors));

            var resultRemocao = await usuarioService.RemoverConfirmacaoEmailAsync(usuario);

            if (!resultRemocao.Succeeded)
                return BadRequest(ErrorResponse.From(resultRemocao.Errors));

            await usuarioService.EnviarTokenConfirmacaoEmailAsync(usuario);

            return Accepted();
        }

        /// <summary>
        /// Realiza a alteração de senha.
        /// </summary>
        [HttpPut("alterarSenha")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AlterarSenha(AlteracaoSenhaInformacoes informacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var usuario = await usuarioService.ObterPeloNomeAsync(informacoes.NomeUsuario);

            if (usuario == null)
                return NotFound();

            var result = await usuarioService.AlterarSenhaAsync(usuario, informacoes.SenhaAtual, informacoes.NovaSenha);

            if (!result.Succeeded)
                return BadRequest(ErrorResponse.From(result.Errors));

            return Ok();
        }

        /// <summary>
        /// Define se o usuário irá ou não utilizar autenticação de dois fatores.
        /// </summary>
        [HttpPut("alterarAutenticacaoDoisFatores")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AlterarAutenticacaoDoisFatores(AlterarAutenticacaoDoisFatoresInformacoes informacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var usuario = await usuarioService.ObterPeloNomeAsync(informacoes.NomeUsuario);

            if (usuario == null)
                return NotFound();

            var result = await usuarioService.AlterarAutenticacaoDeDoisFatores(usuario, informacoes.UtilizarAutenticacaoDoisFatores);

            if (!result.Succeeded)
                return BadRequest(ErrorResponse.From(result.Errors));

            return Ok();
        }

        /// <summary>
        /// Realiza a alteração de senha através do token.
        /// </summary>
        [HttpPut("redefinirSenha")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RedefinirSenha(RedefinicaoSenhaInformacoes informacoes)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var usuario = await usuarioService.ObterPeloNomeAsync(informacoes.NomeUsuario);

            if (usuario == null)
                return NotFound();

            var result = await usuarioService.RedefinirSenhaAsync(usuario, informacoes.Token, informacoes.NovaSenha);

            if (!result.Succeeded)
                return BadRequest(ErrorResponse.From(result.Errors));

            return Ok();
        }

        /// <summary>
        /// Realiza a exclusão do usuário com o nome informado.
        /// </summary>
        [HttpDelete("{nomeUsuario}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remover(string nomeUsuario)
        {
            var usuario = await usuarioService.ObterPeloNomeAsync(nomeUsuario);

            if (usuario == null)
                return NotFound();

            var result = await usuarioService.RemoverAsync(usuario);

            if (!result.Succeeded)
                return BadRequest(ErrorResponse.From(result.Errors));

            return NoContent();
        }
    }
}
