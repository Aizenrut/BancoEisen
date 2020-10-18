using BancoEisen.Models.Enumeracoes;
using BancoEisen.Models.Operacoes;

namespace BancoEisen.Data.Models
{
    public class TransferenciaFiltro : OperacaoFiltro, IFiltro<Transferencia>
    {
        public TipoOperacao TipoOperacao { get; } = TipoOperacao.Transferencia;
    }
}
