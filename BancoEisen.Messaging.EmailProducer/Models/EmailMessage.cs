using System.ComponentModel.DataAnnotations;

namespace BancoEisen.Messaging.EmailProducer.Models
{
    public struct EmailMessage
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Para { get; set; }

        [Required]
        public string Assunto { get; set; }
        
        [Required]
        public string Conteudo { get; set; }
    }
}
