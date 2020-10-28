using BancoEisen.Models.Abstracoes;
using BancoEisen.Models.Cadastros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BancoEisen.Data.Contexts
{
    public class BancoEisenContext : DbContext
    {
        private static bool tabelasCriadas;

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Operacao> Operacoes { get; set; }

        public BancoEisenContext(DbContextOptions<BancoEisenContext> options) : base(options)
        {
            this.Database.EnsureCreated();

            if (!tabelasCriadas)
            {
                var creator = (RelationalDatabaseCreator)this.Database.GetService<IDatabaseCreator>();
                creator.CreateTables();

                tabelasCriadas = true;
            }
        }
    }
}
