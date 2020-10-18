using BancoEisen.Data.Contexts;
using BancoEisen.Data.Models;
using BancoEisen.Data.Services.Interfaces;
using BancoEisen.Models.Abstracoes;

namespace BancoEisen.Data.Repositories
{
    public class OperacaoRepository : Repository<Operacao, OperacaoFiltro>, IOperacaoRepository
    {
        public OperacaoRepository(BancoEisenContext context, IFiltragemService<Operacao, OperacaoFiltro> filtragemService, IOrdenacaoService<Operacao> ordenacaoService)
            : base(context, filtragemService, ordenacaoService)
        {
        }
    }
}
