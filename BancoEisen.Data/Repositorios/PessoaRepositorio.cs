using BancoEisen.Data.Contextos;
using BancoEisen.Data.Models.Filtros;
using BancoEisen.Data.Repositorios.Interfaces;
using BancoEisen.Data.Services.Interfaces;
using BancoEisen.Models.Cadastros;

namespace BancoEisen.Data.Repositorios
{
    public class PessoaRepositorio : Repositorio<Pessoa, PessoaFiltro>, IPessoaRepositorio
    {
        public PessoaRepositorio(BancoEisenContext context, IFiltragemService<Pessoa, PessoaFiltro> filtragemService, IOrdenacaoService<Pessoa> ordenacaoService)
            : base(context, filtragemService, ordenacaoService)
        {
        }
    }
}
