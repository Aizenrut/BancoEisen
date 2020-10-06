using BancoEisen.Models.Abstracoes;
using BancoEisen.API.Extensions;
using System;

namespace BancoEisen.API.Models.Operacoes
{
    public class OperacaoResource
    {
        public int Id { get; }
        public decimal Valor { get; }
        public string TipoOperacao { get; set; }
        public string Observacao { get; set; }
        public DateTime DataEfetivacao { get; set; }

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
