using BancoEisen.Data.Models.Filtros.Abstracoes;
using BancoEisen.Data.Models.Filtros.Interfaces;
using BancoEisen.Models.Enumeracoes;
using BancoEisen.Models.Operacoes;

namespace BancoEisen.Data.Models.Filtros
{
    public class TransferenciaFiltro : OperacaoFiltro, IFiltro<Transferencia>
    {
        public TipoOperacao TipoOperacao { get; } = TipoOperacao.Transferencia;
    }
}
