using BancoEisen.Data.Contextos;
using BancoEisen.Data.Models.Filtros.Abstracoes;
using BancoEisen.Data.Repositorios.Interfaces;
using BancoEisen.Data.Services.Interfaces;
using BancoEisen.Models.Abstracoes;

namespace BancoEisen.Data.Repositorios
{
    public class OperacaoRepositorio : Repositorio<Operacao, OperacaoFiltro>, IOperacaoRepositorio
    {
        public OperacaoRepositorio(BancoEisenContext context, IFiltragemService<Operacao, OperacaoFiltro> filtragemService, IOrdenacaoService<Operacao> ordenacaoService)
            : base(context, filtragemService, ordenacaoService)
        {
        }
    }
}
