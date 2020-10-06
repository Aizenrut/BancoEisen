using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BancoEisen.Models.Informacoes
{
    public struct ContaInformacoes
    {
        [Required]
        public int Agencia { get; set; }

        [Required]
        public int Numero { get; set; }

        [Required]
        public byte Digito { get; set; }

        [Required]
        public int TitularId { get; set; }

        [DefaultValue(0)]
        public decimal Saldo { get; set; }
    }
}
