using BancoEisen.Models.Abstracoes;
using System.Linq.Expressions;
using System.Linq;
using System;

namespace BancoEisen.Data
{
    public class Repository<T> : IRepository<T> where T : Entidade
    {
        private readonly BancoEisenContext context;

        public Repository(BancoEisenContext context)
        {
            this.context = context;
        }

        public T[] All()
        {
            return context.Set<T>().Select(entity => entity).ToArray();
        }

        public bool Any(int id)
        {
            return Any(x => x.Id == id);
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return context.Set<T>().Any(expression);
        }

        public T Get(int id)
        {
            return context.Find<T>(new object[] { id });
        }

        public T[] Where(Expression<Func<T, bool>> expression)
        {
            return context.Set<T>().Where(expression).ToArray();
        }
        
        public T Post(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();

            return entity;
        }

        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }
    }
}
