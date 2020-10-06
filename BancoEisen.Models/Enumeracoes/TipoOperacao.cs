using System.ComponentModel;

namespace BancoEisen.Models.Enumeracoes
{
    public enum TipoOperacao
    {
        [Description("Depósito")]
        Deposito,
        [Description("Saque")]
        Saque,
        [Description("Transferência")]
        Transferencia
    }
}
