using System.ComponentModel.DataAnnotations;

namespace BancoEisen.Models.Informacoes
{
    public struct OperacaoUnariaInformacoes
    {
        [Required]
        public int ContaId { get; set; }

        [Required]
        public decimal Valor { get; set; }
        
        [MaxLength(100)]
        public string Observacao { get; set; }

        public OperacaoUnariaInformacoes(int contaId, decimal valor, string observacao = "")
        {
            ContaId = contaId;
            Valor = valor;
            Observacao = observacao;
        }
    }
}
