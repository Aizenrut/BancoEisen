using BancoEisen.Models.Abstracoes;
using BancoEisen.Models.Cadastros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace BancoEisen.Data.Contexts
{
    public class BancoEisenContext : DbContext
    {
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Operacao> Operacoes { get; set; }

        public BancoEisenContext(DbContextOptions<BancoEisenContext> options) : base(options)
        {
            if (!this.Database.EnsureCreated())
            {
                try
                {
                    var creator = (RelationalDatabaseCreator)this.Database.GetService<IDatabaseCreator>();
                    creator.CreateTables();
                }
                catch
                {
                }
            }
        }
    }
}
