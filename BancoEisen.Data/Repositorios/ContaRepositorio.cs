using BancoEisen.Data.Contextos;
using BancoEisen.Data.Models.Filtros;
using BancoEisen.Data.Repositorios.Interfaces;
using BancoEisen.Data.Services.Interfaces;
using BancoEisen.Models.Cadastros;

namespace BancoEisen.Data.Repositorios
{
    public class ContaRepositorio : Repositorio<Conta, ContaFiltro>, IContaRepositorio
    {
        public ContaRepositorio(BancoEisenContext context, IFiltragemService<Conta, ContaFiltro> filtragemService, IOrdenacaoService<Conta> ordenacaoService)
            : base(context, filtragemService, ordenacaoService)
        {
        }
    }
}
