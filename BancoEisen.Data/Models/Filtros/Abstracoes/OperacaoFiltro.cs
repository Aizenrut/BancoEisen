using BancoEisen.Data.Models.Filtros.Interfaces;
using BancoEisen.Models.Abstracoes;
using System;

namespace BancoEisen.Data.Models.Filtros.Abstracoes
{
    public class OperacaoFiltro : IFiltro<Operacao>
    {
        public DateTime DataEfetivacao { get; set; }
    }
}
