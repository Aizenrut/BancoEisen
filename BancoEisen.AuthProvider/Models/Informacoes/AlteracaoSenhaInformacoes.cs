using System.ComponentModel.DataAnnotations;

namespace BancoEisen.AuthProvider.Models
{
    public class AlteracaoSenhaInformacoes
    {
        [Required]
        public string NomeUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string SenhaAtual { get; set; }

        [Required]
        public string NovaSenha { get; set; }
    }
}
