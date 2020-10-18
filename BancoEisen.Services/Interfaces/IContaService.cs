using BancoEisen.Data.Models;
using BancoEisen.Models.Cadastros;
using BancoEisen.Models.Informacoes;

namespace BancoEisen.Services
{
    public interface IContaService : ICadastroService<Conta, ContaInformacoes, ContaFiltro>
    {
    }
}
