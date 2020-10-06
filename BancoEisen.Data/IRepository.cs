using System.Linq.Expressions;
using System;

namespace BancoEisen.Data
{
    public interface IRepository<T>
    {
        T[] All();
        bool Any(int id);
        bool Any(Expression<Func<T, bool>> expression);
        T Get(int id);
        T[] Where(Expression<Func<T, bool>> expression);
        T Post(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
