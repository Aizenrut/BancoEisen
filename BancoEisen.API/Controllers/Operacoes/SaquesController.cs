using BancoEisen.API.Controllers.Templates;
using BancoEisen.API.Services;
using BancoEisen.Data.Models;
using BancoEisen.Models.Informacoes;
using BancoEisen.Services;
using Microsoft.AspNetCore.Http;

namespace BancoEisen.API.Controllers.Operacoes
{
    public class SaquesController : OperacoesControllerTamplate<ISaqueService, OperacaoUnariaInformacoes, SaqueFiltro>
    {
        public SaquesController(ISaqueService servico, IPaginacaoService paginacaoService, IHttpContextAccessor contextAccessor) 
            : base(servico, paginacaoService, contextAccessor)
        {
        }
    }
}
