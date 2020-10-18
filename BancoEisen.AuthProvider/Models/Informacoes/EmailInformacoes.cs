using System.ComponentModel.DataAnnotations;

namespace BancoEisen.AuthProvider.Models.Informacoes
{
    public struct EmailInformacoes
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Para { get; }

        [Required]
        public string Assunto { get; }

        [Required]
        public string Conteudo { get; }

        public EmailInformacoes(string para, string assunto, string conteudo)
        {
            Para = para;
            Assunto = assunto;
            Conteudo = conteudo;
        }
    }
}
