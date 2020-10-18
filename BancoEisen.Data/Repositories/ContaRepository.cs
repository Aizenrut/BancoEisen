using BancoEisen.Data.Contexts;
using BancoEisen.Data.Models;
using BancoEisen.Data.Services.Interfaces;
using BancoEisen.Models.Cadastros;

namespace BancoEisen.Data.Repositories
{
    public class ContaRepository : Repository<Conta, ContaFiltro>, IContaRepository
    {
        public ContaRepository(BancoEisenContext context, IFiltragemService<Conta, ContaFiltro> filtragemService, IOrdenacaoService<Conta> ordenacaoService)
            : base(context, filtragemService, ordenacaoService)
        {
        }
    }
}
