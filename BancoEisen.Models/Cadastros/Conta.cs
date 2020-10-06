using System.ComponentModel;
using System.Collections.Generic;
using BancoEisen.Models.Abstracoes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace BancoEisen.Models.Cadastros
{
    [Table("Contas")]
    public class Conta : Entidade
    {
        [Required]
        public int Agencia { get; set; }

        [Required]
        public int Numero { get; set; }

        [Required]
        public byte Digito { get; set; }

        [Required]
        public int TitularId { get; set; }
        public Pessoa Titular { get; set; }

        [DefaultValue(0)]
        public decimal Saldo { get; set; }

        public DateTime DataAbertura { get; set; }

        public ICollection<Operacao> Operacoes { get; set; }

        public Conta()
        {

        }

        public Conta(int agencia, int numero, byte digito, int titularId)
        {
            Agencia = agencia;
            Numero = numero;
            Digito = digito;
            TitularId = titularId;
            DataAbertura = DateTime.Now;

            Operacoes = new List<Operacao>();
        }
    }
}
