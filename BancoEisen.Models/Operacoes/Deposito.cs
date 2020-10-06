using BancoEisen.Models.Abstracoes;
using BancoEisen.Models.Enumeracoes;

namespace BancoEisen.Models.Operacoes
{
    public class Deposito : Operacao
    {
        public Deposito() : base()
        {
        }

        public Deposito(decimal valor, string observacao = "") : base(valor, TipoOperacao.Deposito, observacao)
        {
        }

    }
}
