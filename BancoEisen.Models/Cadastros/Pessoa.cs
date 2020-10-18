using System;
using BancoEisen.Models.Abstracoes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoEisen.Models.Cadastros
{
    [Table("Pessoas")]
    public class Pessoa : Entidade
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Sobrenome { get; set; }

        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        public string Cpf { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public Pessoa()
        {

        }

        public Pessoa(string nome, string sobrenome, string cpf, DateTime dataNascimento, string email)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            Email = email;
        }
    }
}
