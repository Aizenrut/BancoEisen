using BancoEisen.Models.Enumeracoes;
using BancoEisen.Models.Operacoes;

namespace BancoEisen.Data.Models
{
    public class SaqueFiltro : OperacaoFiltro, IFiltro<Saque>
    {
        public TipoOperacao TipoOperacao { get; } = TipoOperacao.Saque;
    }
}
