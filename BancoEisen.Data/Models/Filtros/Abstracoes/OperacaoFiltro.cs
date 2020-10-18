using BancoEisen.Models.Abstracoes;
using System;

namespace BancoEisen.Data.Models
{
    public class OperacaoFiltro : IFiltro<Operacao>
    {
        public DateTime DataEfetivacao { get; set; }
    }
}
