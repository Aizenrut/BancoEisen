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
        public decimal Valor { get; set; }

        [MaxLength(100)]
        public string Observacao { get; set; }
    }
}
