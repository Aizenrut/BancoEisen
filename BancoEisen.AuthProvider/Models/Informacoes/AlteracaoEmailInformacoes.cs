using System.ComponentModel.DataAnnotations;

namespace BancoEisen.AuthProvider.Models
{
    public struct AlteracaoEmailInformacoes
    {
        [Required]
        public string NomeUsuario { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string NovoEmail { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
