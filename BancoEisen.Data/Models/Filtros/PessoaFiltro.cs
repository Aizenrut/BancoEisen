using BancoEisen.Data.Models.Filtros.Interfaces;
using BancoEisen.Models.Cadastros;
using System;

namespace BancoEisen.Data.Models.Filtros
{
    public class PessoaFiltro : IFiltro<Pessoa>
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public int UsuarioId { get; set; }
    }
}
