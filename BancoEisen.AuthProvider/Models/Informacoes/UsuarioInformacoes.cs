using System.ComponentModel.DataAnnotations;

namespace BancoEisen.AuthProvider.Models
{
    public struct UsuarioInformacoes
    {
        [Required]
        public string NomeUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
