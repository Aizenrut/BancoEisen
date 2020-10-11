using System;
using BancoEisen.Models.Enumeracoes;
using System.ComponentModel.DataAnnotations;

namespace BancoEisen.Models.Abstracoes
{
    public class Operacao : Entidade
    {
        [Required]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        [Required]
        public TipoOperacao TipoOperacao { get; set; }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DataEfetivacao { get; set; }

        public Operacao()
        {

        }

        public Operacao(decimal valor, TipoOperacao tipoOperacao, string observacao = "")
        {
            Valor = valor;
            TipoOperacao = tipoOperacao;
            Observacao = observacao;
            DataEfetivacao = DateTime.Now;
        }
    }
}
