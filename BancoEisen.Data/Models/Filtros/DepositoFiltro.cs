using BancoEisen.Models.Enumeracoes;
using BancoEisen.Models.Operacoes;

namespace BancoEisen.Data.Models
{
    public class DepositoFiltro : OperacaoFiltro, IFiltro<Deposito>
    {
        public TipoOperacao TipoOperacao { get; } = TipoOperacao.Deposito;
    }
}
