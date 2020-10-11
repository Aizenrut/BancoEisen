using BancoEisen.Data.Contextos;
using BancoEisen.Data.Models.Filtros.Interfaces;
using BancoEisen.Data.Models.Ordens;
using BancoEisen.Data.Repositorios.Interfaces;
using BancoEisen.Data.Services.Interfaces;
using BancoEisen.Models.Abstracoes;
using System.Linq;
using System.Threading.Tasks;

namespace BancoEisen.Data.Repositorios
{
    public abstract class Repositorio<TEntidade, TFiltro> : IRepositorioFiltravel<TEntidade, TFiltro>, IRepositorioOrdenavel<TEntidade>
        where TEntidade : Entidade
        where TFiltro : IFiltro<TEntidade>
    {
        protected readonly BancoEisenContext context;
        protected readonly IFiltragemService<TEntidade, TFiltro> filtragemService;
        protected readonly IOrdenacaoService<TEntidade> ordenacaoService;

        public Repositorio(BancoEisenContext context,
                           IFiltragemService<TEntidade, TFiltro> filtragemService,
                           IOrdenacaoService<TEntidade> ordenacaoService)
        {
            this.context = context;
            this.filtragemService = filtragemService;
            this.ordenacaoService = ordenacaoService;
        }

        public IQueryable<TEntidade> All()
        {
            return context.Set<TEntidade>().AsQueryable<TEntidade>();
        }

        public bool Any(int id)
        {
            return context.Set<TEntidade>().Any(x => x.Id == id);
        }

        public async Task DeleteAsync(TEntidade entity)
        {
            context.Set<TEntidade>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public IQueryable<TEntidade> Filtrar(IQueryable<TEntidade> query, TFiltro filtro)
        {
            return filtragemService.Filtrar(query, filtro);
        }

        public TEntidade Get(int id)
        {
            return context.Set<TEntidade>().FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<TEntidade> Ordenar(IQueryable<TEntidade> query, Ordem ordem)
        {
            return ordenacaoService.Ordenar(query, ordem);
        }

        public async Task PostAsync(TEntidade entity)
        {
            context.Set<TEntidade>().Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntidade entity)
        {
            context.Set<TEntidade>().Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
