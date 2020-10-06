using BancoEisen.API.Controllers.Templates;
using BancoEisen.Controllers.Interfaces;
using BancoEisen.Models.Informacoes;
using Microsoft.AspNetCore.Mvc;

namespace BancoEisen.API.Controllers.Operacoes
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferenciasController : OperacoesControllerTamplate<ITransferenciaController, OperacaoBinariaInformacoes>
    {
        public TransferenciasController(ITransferenciaController transferenciaServico) : base(transferenciaServico)
        {
        }
    }
}
