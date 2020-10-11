using BancoEisen.Models.Abstracoes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoEisen.Models.Cadastros
{
    [Table("Usuarios")]
    public class Usuario : Entidade
    {
        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string Login { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(72)]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        public Usuario()
        {

        }

        public Usuario(string login, string senha)
        {
            Login = login;
            Senha = senha;
        }
    }
}
