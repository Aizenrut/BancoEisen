using BancoEisen.Data.Models.Filtros.Abstracoes;
using BancoEisen.Data.Models.Filtros.Interfaces;
using BancoEisen.Models.Enumeracoes;
using BancoEisen.Models.Operacoes;

namespace BancoEisen.Data.Models.Filtros
{
    public class DepositoFiltro : OperacaoFiltro, IFiltro<Deposito>
    {
        public TipoOperacao TipoOperacao { get; } = TipoOperacao.Deposito;
    }
}
