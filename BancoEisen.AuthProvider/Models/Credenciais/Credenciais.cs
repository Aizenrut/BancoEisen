using System.ComponentModel.DataAnnotations;

namespace BancoEisen.AuthProvider.Models
{
    public struct Credenciais
    {
        [Required]
        public string NomeUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}
