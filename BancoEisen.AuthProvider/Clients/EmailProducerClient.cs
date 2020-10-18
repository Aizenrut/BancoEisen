using BancoEisen.AuthProvider.Models.Informacoes;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BancoEisen.AuthProvider.Clients
{
    public class EmailProducerClient
    {
        private readonly HttpClient client;

        public EmailProducerClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task EnfileirarEmailAsync(string para, string assunto, string conteudo)
        {
            var informacoes = new EmailInformacoes(para, assunto, conteudo);

            var content = new StringContent(content: JsonConvert.SerializeObject(informacoes),
                                            encoding: Encoding.UTF8,
                                            mediaType: "application/json");

            await client.PostAsync("EmailProducer", content);
        }
    }
}
