using BancoEisen.AuthProvider.Models;
using BancoEisen.AuthProvider.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BancoEisen.AuthProvider.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1.0")]
    [Consumes("application/json", "text/json")]
    [Produces("application/json", "text/json", "application/xml", "text/xml")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;

        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        /// <summary>
        /// Realiza a autenticação e geração do token caso o usuário possua apenas um fator de autenticação.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status423Locked)]
        public async Task<IActionResult> Autenticar(Credenciais credenciais)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var result = await loginService.AutenticarAsync(credenciais);

            return await TratarSignInResult(result, credenciais);
        }

        /// <summary>
        /// Realiza a autenticação de dois fatores e geração do token caso o usuário possua dois fatores de autenticação.
        /// </summary>
        [HttpPatch]
        public async Task<IActionResult> AutenticarDoisFatores(CredenciaisDoisFatores credenciais)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var result = await loginService.AutenticarDoisFatoresAsync(credenciais);

            return TratarSignInResult(result, credenciais);
        }

        private async Task<IActionResult> TratarSignInResult(SignInResult result, Credenciais credenciais)
        {
            if (result.Succeeded)
                return Ok(new TokenResponse(loginService.GerarToken(credenciais.NomeUsuario)));

            if (result.RequiresTwoFactor)
            {
                await loginService.EnviarTokenAutenticacaoDoisFatoresAsync(credenciais.NomeUsuario);
                return Accepted();
            }

            if (result.IsNotAllowed)
                return StatusCode(403);

            if (result.IsLockedOut)
                return await TratarLockout(credenciais.NomeUsuario, credenciais.Senha);

            return Unauthorized();
        }

        private IActionResult TratarSignInResult(SignInResult result, CredenciaisDoisFatores credenciais)
        {
            if (result.Succeeded)
                return Ok(new TokenResponse(loginService.GerarToken(credenciais.NomeUsuario)));

            return Unauthorized();
        }

        private async Task<IActionResult> TratarLockout(string usuario, string senha)
        {
            var ehCorreta = await loginService.EhSenhaCorretaAsync(usuario, senha);

            if (ehCorreta)
                return StatusCode(423);

            return Unauthorized();
        }
    }
}
