using BancoEisen.Models.Cadastros;
using System;

namespace BancoEisen.API.Models
{
    public struct ContaResource
    {
        public int Id { get; }
        public int Agencia { get; }
        public int Numero { get; }
        public byte Digito { get; }
        public string Titular { get; }
        public decimal Saldo { get; set; }
        public DateTime DataAbertura { get; }

        public ContaResource(Conta conta)
        {
            Id = conta.Id;
            Agencia = conta.Agencia;
            Numero = conta.Numero;
            Digito = conta.Digito;
            Titular = $"/api/Pessoas/{conta.TitularId}";
            Saldo = conta.Saldo;
            DataAbertura = conta.DataAbertura;
        }
    }
}
