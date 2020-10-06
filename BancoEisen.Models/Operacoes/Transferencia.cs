using BancoEisen.Models.Abstracoes;
using BancoEisen.Models.Enumeracoes;

namespace BancoEisen.Models.Operacoes
{
    public class Transferencia : Operacao
    {
        public Transferencia() : base()
        {
        }

        public Transferencia(decimal valor, string observacao = "") : base(valor, TipoOperacao.Transferencia, observacao)
        {
        }
    }
}
