using System.ComponentModel.DataAnnotations;

namespace BancoEisen.AuthProvider.Models
{
    public struct RedefinicaoSenhaInformacoes
    {
        [Required]
        public string NomeUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NovaSenha { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
