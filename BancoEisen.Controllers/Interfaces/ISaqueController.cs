using BancoEisen.Data.Models.Filtros;
using BancoEisen.Models.Informacoes;

namespace BancoEisen.Controllers.Interfaces
{
    public interface ISaqueController : IOperacaoController<OperacaoUnariaInformacoes, SaqueFiltro>
    {
    }
}