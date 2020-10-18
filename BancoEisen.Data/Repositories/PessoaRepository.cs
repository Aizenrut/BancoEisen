using BancoEisen.Data.Contexts;
using BancoEisen.Data.Models;
using BancoEisen.Data.Services.Interfaces;
using BancoEisen.Models.Cadastros;

namespace BancoEisen.Data.Repositories
{
    public class PessoaRepository : Repository<Pessoa, PessoaFiltro>, IPessoaRepository
    {
        public PessoaRepository(BancoEisenContext context, IFiltragemService<Pessoa, PessoaFiltro> filtragemService, IOrdenacaoService<Pessoa> ordenacaoService)
            : base(context, filtragemService, ordenacaoService)
        {
        }
    }
}
