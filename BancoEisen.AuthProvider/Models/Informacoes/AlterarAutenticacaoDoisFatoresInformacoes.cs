using System.ComponentModel.DataAnnotations;

namespace BancoEisen.AuthProvider.Models
{
    public struct AlterarAutenticacaoDoisFatoresInformacoes
    {
        [Required]
        public string NomeUsuario { get; set; }
        
        [Required]
        public bool UtilizarAutenticacaoDoisFatores { get; set; }
    }
}
