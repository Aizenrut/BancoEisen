using BancoEisen.Models.Abstracoes;
using BancoEisen.Models.Enumeracoes;

namespace BancoEisen.Models.Operacoes
{
    public class Saque : Operacao
    {
        public Saque() : base()
        {
        }

        public Saque(decimal valor, string observacao = "") : base(valor, TipoOperacao.Saque, observacao)
        {
        }
    }
}
