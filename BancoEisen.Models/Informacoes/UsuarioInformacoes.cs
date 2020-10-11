using System.ComponentModel.DataAnnotations;

namespace BancoEisen.Models.Informacoes
{
    public struct UsuarioInformacoes
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
    }
}
