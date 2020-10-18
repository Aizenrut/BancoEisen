using System.Linq;
using System.Threading.Tasks;

namespace BancoEisen.Data.Repositories
{
    public interface IRepositorioBase<T>
    {
        IQueryable<T> All();
        bool Any(int id);
        T Get(int id);
        Task PostAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
