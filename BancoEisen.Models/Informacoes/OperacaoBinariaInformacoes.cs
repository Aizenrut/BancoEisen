using System.ComponentModel.DataAnnotations;

namespace BancoEisen.Models.Informacoes
{
    public struct OperacaoBinariaInformacoes
    {
        [Required]
        public int ContaOrigemId { get; set; }

        [Required]
        public int ContaDestinoId { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        [MaxLength(100)]
        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }
    }
}
