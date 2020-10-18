using System.ComponentModel.DataAnnotations;

namespace BancoEisen.AuthProvider.Models
{
    public struct ConfirmacaoInformacoes
    {
        [Required]
        public string NomeUsuario { get; set; }
        
        [Required]
        public string Token { get; set; }
    }
}
