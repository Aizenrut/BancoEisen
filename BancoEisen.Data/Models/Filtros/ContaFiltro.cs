using BancoEisen.Data.Models.Filtros.Interfaces;
using BancoEisen.Models.Cadastros;
using System;

namespace BancoEisen.Data.Models.Filtros
{
    public class ContaFiltro : IFiltro<Conta>
    {
        public int Agencia { get; set; }
        public int Numero { get; set; }
        public byte Digito { get; set; }
        public int TitularId { get; set; }
        public DateTime DataAbertura { get; set; }
    }
}
