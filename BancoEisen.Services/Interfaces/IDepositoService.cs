using BancoEisen.Data.Models;
using BancoEisen.Models.Informacoes;

namespace BancoEisen.Services
{
    public interface IDepositoService : IOperacaoService<OperacaoUnariaInformacoes, DepositoFiltro>
    {
    }
}