using BancoEisen.Data.Models;
using BancoEisen.Models.Cadastros;
using BancoEisen.Models.Informacoes;

namespace BancoEisen.Services
{
    public interface IPessoaService : ICadastroService<Pessoa, PessoaInformacoes, PessoaFiltro>
    {
    }
}
