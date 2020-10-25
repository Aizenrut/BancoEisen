using BancoEisen.AuthProvider.Models;
using BancoEisen.AuthProvider.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BancoEisen.AuthProvider.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;

        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Autenticar(Credenciais credenciais)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.From(ModelState));

            var result = await loginService.AutenticarAsync(credenciais);

            return await TratarSignInResult(result, credenciais);
        }

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
                return Ok(new { token = loginService.GerarToken(credenciais.NomeUsuario) });

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
                return Ok(new { token = loginService.GerarToken(credenciais.NomeUsuario) });

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
