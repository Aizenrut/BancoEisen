using BancoEisen.Models.Abstracoes;
using BancoEisen.API.Extensions;
using System;

namespace BancoEisen.API.Models
{
    public class OperacaoResource
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public string TipoOperacao { get; set; }
        public string Observacao { get; set; }
        public DateTime DataEfetivacao { get; set; }

        public OperacaoResource()
        {

        }

        public OperacaoResource(Operacao operacao)
        {
            Id = operacao.Id;
            Valor = operacao.Valor;
            TipoOperacao = operacao.TipoOperacao.GetDescription();
            Observacao = operacao.Observacao;
            DataEfetivacao = operacao.DataEfetivacao;
        }
    }
}
